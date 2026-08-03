using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.VisualElements;
using Models.Dto;
using Models.Entities;
using SkiaSharp;
using Task = System.Threading.Tasks.Task;

namespace DesktopApp.ViewModels.Pages;

public partial class MainPageViewModel : ViewModelBase
{
    [ObservableProperty] private List<News> _news;
    public List<Sell> Sells { get; set; }
    [ObservableProperty] private MoneyInfo _moneyInfo;
    [ObservableProperty] private StatusesInfo _statusesInfo;
    [ObservableProperty] private int _machinesAmount = 0;
    [ObservableProperty] private ServicesInfo _servicesInfo;
    [ObservableProperty] private decimal _cashCollectionsYesterday;
    [ObservableProperty] private decimal _cashCollectionsToday;
    [ObservableProperty] private decimal _incomeToday;
    [ObservableProperty] private string _datesText;
    [ObservableProperty] private decimal _incomeYesterday;
    [ObservableProperty] private PieSeries<ObservableValue>[] _efficientBlockSeries;
    [ObservableProperty] private ColumnSeries<DateTimePoint>[] _dynamicBlockSeries;
    [ObservableProperty] private List<PieSeries<int>> _netStateBlockSeries;
    [ObservableProperty] private List<VisualElement> _efficientBlockVisual;
    private bool _showSellsBySumFlag;

    public bool ShowSellsBySumFlag
    {
        get => _showSellsBySumFlag;
        set
        {
            CreateSellsChart(value);
            SetProperty(ref _showSellsBySumFlag, value);
        }
    }

    private HttpClient _httpClient;

    public MainPageViewModel(HttpClient client)
    {
        _httpClient = client;
    }

    [RelayCommand]
    public void ShowSellsBySum()
    {
        ShowSellsBySumFlag = true;
    }

    [RelayCommand]
    public void ShowSellsByQuantity()
    {
        ShowSellsBySumFlag = false;
    }

    private void SetSeriesStyle(PieSeries<ObservableValue> series, Paint color)
    {
        series.MaxRadialColumnWidth = 30;
        series.InnerRadius = 50;
        series.CornerRadius = 0;
        series.Fill = color;
    }

    [RelayCommand]
    public void CreateCharts()
    {
        ShowSellsBySumFlag = true;
        EfficientBlockSeries = GaugeGenerator
            .BuildAngularGaugeSections(
                new GaugeItem(40, series => SetSeriesStyle(series, SolidColorPaint.Parse("#FFADAD"))),
                new GaugeItem(30, series => SetSeriesStyle(series, SolidColorPaint.Parse("#FFD6A5"))),
                new GaugeItem(30, series => SetSeriesStyle(series, SolidColorPaint.Parse("#CAFFBF"))));
        EfficientBlockVisual = new List<VisualElement>()
        {
            new AngularTicksVisual()
            {
                LabelsSize = 16
            },
            new NeedleVisual()
            {
                Value = StatusesInfo.WorkingPercent,
            }
        };
        NetStateBlockSeries = new List<PieSeries<int>>()
        {
            new PieSeries<int>()
            {
                Values = [StatusesInfo.WorkingAmount],
                Name = "Работает",
                Fill = new SolidColorPaint(SKColors.LightGreen)
            },
            new PieSeries<int>()
            {
                Values = [StatusesInfo.InServiceAmount],
                Name = "На обслуживании",
                Fill = new SolidColorPaint(SKColors.LightSkyBlue)
            },
            new PieSeries<int>()
            {
                Values = [StatusesInfo.NotWorkingAmount],
                Name = "Не работает",
                Fill = new SolidColorPaint(SKColors.LightCoral)
            }
        };
    }


    void CreateSellsChart(bool showSum = true)
    {
        ObservableCollection<DateTimePoint> points = new ObservableCollection<DateTimePoint>();
        var today = DateTime.Now.Date;
        var currentDay = today.AddDays(-9);
        for (; currentDay <= today; currentDay = currentDay.AddDays(1))
        {
            var sellsOnCurrentDay = Sells.FindAll(x => x.DateTime.Date == currentDay);
            decimal value = showSum
                ? sellsOnCurrentDay.Sum(x => x.Income) - sellsOnCurrentDay.Sum(x => x.Change)
                : sellsOnCurrentDay.Count;
            points.Add(new DateTimePoint(currentDay, (double)value));
        }
        DynamicBlockSeries = new []
        {
            new ColumnSeries<DateTimePoint>()
            {
                MaxBarWidth = 30,
                Values = points,
            }
        };
    }

    public ICartesianAxis[] AxisArray => [
        new DateTimeAxis(TimeSpan.FromDays(1), d => d.ToString("dd MMMM"))
        {
        }
    ];

    [RelayCommand]
    async System.Threading.Tasks.Task LoadDataAsync()
    {
        var statusesTask = _httpClient.SafeGetAsync<StatusesInfo>("/machines/statuses-info");
        var newsTask = _httpClient.SafeGetAsync<List<News>>("/news");
        var moneyInfoTask = _httpClient.SafeGetAsync<MoneyInfo>("/machines/money-info");
        var servicesInfoTask = _httpClient.SafeGetAsync<ServicesInfo>("/machines/services-info");
        var sellsTask = _httpClient.SafeGetAsync<List<Sell>>("/sells/last-10-days");
        var cashCollectionsTask = _httpClient.SafeGetAsync<List<CashCollection>>("/cash-collections");

        var (statuses, statusesError) = await statusesTask;
        var (news, newsError) = await newsTask;
        var (money, moneyError) = await moneyInfoTask;
        var (services, servicesError) = await servicesInfoTask;
        var (sells, sellsError) = await sellsTask;
        var (cash, cashError) = await cashCollectionsTask;

        if (statusesError != null || newsError != null || moneyError != null ||
            servicesError != null || sellsError != null || cashError != null)
        {
            await ShowError("Ошибка", "Во время загрузки данных произошла ошибка, попробуйте загрузить данные позже");
        }
        else
        {
            StatusesInfo = statuses;
            Sells = sells;
            MoneyInfo = money;
            News = news;
            ServicesInfo = services;
            var today = DateTime.Now.Date;
            var yesterday = today.AddDays(-1);
            var nineDaysAgo = today.AddDays(-9);
            DatesText = $"Данные по продажам с {nineDaysAgo.ToShortDateString()} по {today.ToShortDateString()}";
            IncomeToday = sells.Where(s => s.DateTime.Date == today).Sum(x => x.Income);
            IncomeYesterday = sells.Where(s => s.DateTime.Date == yesterday).Sum(x => x.Income);
            CashCollectionsToday = cash.Where(s => s.DateTime.Date == today).Sum(x => x.TakenSum);
            CashCollectionsYesterday = cash.Where(s => s.DateTime.Date == yesterday).Sum(x => x.TakenSum);

            MachinesAmount = StatusesInfo.WorkingAmount + StatusesInfo.NotWorkingAmount + StatusesInfo.InServiceAmount;
        }
    }
}