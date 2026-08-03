using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using DesktopApp.Services;
using DesktopApp.Util;
using DesktopApp.Views.Windows;
using MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Models.Domain;
using Models.Dto;
using Models.Entities;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using Color = Avalonia.Media.Color;
using Colors = Avalonia.Media.Colors;
using Task = System.Threading.Tasks.Task;

namespace DesktopApp.ViewModels.Pages;

public partial class VendingMachinesPageViewModel : ViewModelBase
{
    private HttpClient _httpClient;
    private MenuNavigationService _navigationService;

    public VendingMachinesPageViewModel(HttpClient client, MenuNavigationService navigationService)
    {
        _httpClient = client;
        _navigationService = navigationService;
    }
    
    [ObservableProperty] private List<VendingMachineShortDto> _machines;
    [ObservableProperty] private string _totalBlockText;
    [ObservableProperty] private List<Page> _pages;
    [ObservableProperty] private int _totalCount;
    [ObservableProperty] private int _totalPagesCount;
    [ObservableProperty] private int _pageNumber = 1;
    public List<int> PageLimits { get; set; } = new List<int>() { 15, 25 };
    
    private string _searchText;
    public string SearchText
    {
        get =>  _searchText;
        set
        {
            _dataChanged = true;
            SetProperty(ref _searchText, value);
            LoadDataAsync();
        }
    }
    
    private int _pageLimit = 15;
    public int PageLimit
    {
        get =>  _pageLimit;
        set
        {
            _dataChanged = true;
            SetProperty(ref _pageLimit, value); 
            LoadDataAsync();
        }
    }
    
    private bool _dataChanged = false;
    
    public async Task<List<VendingMachineShortDto>> LoadAllMachinesForExportAsync()
    {
        var machinesResponse = await _httpClient.SafeGetAsync<List<VendingMachineShortDto>>($"machines/export");
        if (machinesResponse.TryGetResult(out var machines))
        {
           return machines;
        }
        else
        {
            await ShowError("Ошибка", machinesResponse.Exception.Message);
        }
        return new();
    }

    [RelayCommand]
    public async Task LoadDataAsync()
    {
        var query = $"machines/pages?pageNumber={(_dataChanged? 1 : PageNumber)}&pageLimit={PageLimit}";
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query += $"&searchText={Uri.EscapeDataString(SearchText)}";
        }
        var machinesResponse = await _httpClient.SafeGetAsync<VendingMachinesAndCount>(query);

        if (machinesResponse.TryGetResult(out var machinesAndCount))
        {
            Machines = machinesAndCount.VendingMachines;
            TotalCount = machinesAndCount.TotalCount;
            var pages = new List<Page>();
            TotalPagesCount = TotalCount / PageLimit + (TotalCount % PageLimit == 0 ? 0 : 1);
            for (int i = 1; i <= TotalPagesCount; i++)
            {
                var page = new Page()
                {
                    PageNumber = i,
                    Color = Colors.DarkGray.ToString()
                };
                if (page.PageNumber == PageNumber)
                {
                    page.Color = Colors.DodgerBlue.ToString();
                }
                pages.Add(page);
            }
            Pages = pages;
            var firstRow = (PageNumber - 1) * PageLimit + (Machines.Count == 0? 0 : 1);
            TotalBlockText = $"Записи с {firstRow} по {firstRow + Machines.Count - 1}";
            _dataChanged = false;
        }
        else
        {
            await ShowError("Ошибка", "Произошла ошибка во время загрузки данных, повторите загрузку позже");
        }
    }

    [ObservableProperty] private bool _tableRegime = true;
    [RelayCommand] public void ShowInTableRegime() => TableRegime =  true;
    [RelayCommand] public void ShowInPanelRegime() => TableRegime = false;

    [RelayCommand]
    public void Add()
    {
        _navigationService.NavigateTo<VendingMachineFormPageViewModel>();
    }
    [RelayCommand]
    public async Task Edit(int machineId)
    {
        _navigationService.NavigateTo<VendingMachineFormPageViewModel>();
        if (_navigationService.CurrentViewModel is VendingMachineFormPageViewModel formViewModel)
        {
            await formViewModel.InitializeAsync(machineId);
        }
    }

    [RelayCommand]
    private async Task DeleteAsync(int machineId)
    {
        if (await Confirm("Подтверждение операции","Вы уверены, что хотите удалить этот автомат?"))
        {
            try
            {
                var res = await _httpClient.DeleteAsync($"machines/{machineId}");
                if (!res.IsSuccessStatusCode)
                {
                    throw new Exception("Этот автомат нельзя удалить, его данные используются для документов");
                }
                await ShowInfo("Успешная операция",$"Автомат успешно удален");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                await ShowError("Ошибка",$"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }

    [RelayCommand] 
    public async Task ChoosePageNumber(int pageNum)
    {
        if (pageNum <= TotalPagesCount && pageNum > 0)
        {
            PageNumber = pageNum;
            await LoadDataAsync();
        }
    }

    [RelayCommand]
    public async Task NextPage()
    {
        if (PageNumber < TotalPagesCount)
        {
            PageNumber++;
            await LoadDataAsync();
        }   
    }

    [RelayCommand]
    public async Task PreviousPage()
    {
        if (PageNumber > 1)
        {
            PageNumber--;
            await LoadDataAsync();
        }  
    }

    [RelayCommand]
    private async Task Detach(int machineId)
    {
        if (await Confirm("Подтверждение операции","Вы уверены, что хотите отвязать этот автомат от модема?"))
        {
            try
            {
                var res = await _httpClient.PatchAsJsonAsync($"machines/{machineId}/detach", string.Empty);
                if (!res.IsSuccessStatusCode)
                {
                    throw new Exception("Этот автомат нельзя отвязать от модема");
                }
                await ShowInfo("Успешная операция",$"Автомат успешно отвязан от модема");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                await ShowError("Ошибка",$"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }

    [RelayCommand]
    public async Task CsvExport(string csvFilePath)
    {
        var machines = await LoadAllMachinesForExportAsync();
        if (machines.Any())
        {
            using (var file = System.IO.File.Create(csvFilePath))
            {
                using var writer = new StreamWriter(file, Encoding.UTF8);
                using (var csvParser = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvParser.Context.RegisterClassMap<VendingMachineCsvMap>();
                    await csvParser.WriteRecordsAsync(machines);
                }
            }
        }
    }

    [RelayCommand]
    public async Task HtmlExport(string htmlFilePath)
    {
        var machines = await LoadAllMachinesForExportAsync();
        if (machines.Any())
        {
            var result = new StringBuilder();
            result.AppendLine("<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n<meta charset=\"UTF-8\">\n<title>Список торговых автоматов</title>\n</head>\n<body>\n<style>table, tr, th, td { border: solid black 1px; }</style><table>\n<thead>\n<tr>\n<th>ID</th>\n<th>Название автомата</th>\n<th>Модель</th>\n<th>Компания</th>\n<th>Модем</th>\n<th>Адрес/место</th>\n<th>В работе с</th>\n</tr>\n</thead>\n<tbody>");
            foreach (var machine in machines)
            {
                result.Append("<tr>");
                result.Append("<td>" + machine.Id + "</td>");
                result.Append("<td>" + machine.Name + "</td>");
                result.Append("<td>" + machine.ModelName + "</td>");
                result.Append("<td>" + machine.OwnerCompanyName + "</td>");
                result.Append("<td>" + machine.ModemNumber + "</td>");
                result.Append("<td>" + machine.Address + " " + machine.Place + "</td>");
                result.Append("<td>" + machine.DateInstalled.ToShortDateString() + "</td>");
                result.Append("</tr>");
            }
            result.AppendLine("</tbody>\n</table>\n</body>\n</html>");
            using (var file = System.IO.File.Create(htmlFilePath))
            {
                using var writer = new StreamWriter(file, Encoding.UTF8);
                await writer.WriteAsync(result);
            }   
        }
    }

    [RelayCommand]
    public async Task PdfExport(string pdfFilePath)
    {
        var machines = await LoadAllMachinesForExportAsync();
        if (machines.Any())
        {
            var doc = new Document();
            PredefinedFontsAndChars.ErrorFontName = "Arial";
            GlobalFontSettings.FontResolver = new FailsafeFontResolver();
            var section = doc.AddSection();
            var table = section.AddTable();
            table.Borders = new Borders()
            {
                Color = MigraDoc.DocumentObjectModel.Colors.Black,
            };
            table.AddColumn("2cm");
            table.AddColumn("2.5cm");
            table.AddColumn("2.5cm");
            table.AddColumn("2.5cm");
            table.AddColumn("3cm");
            table.AddColumn("3cm");
            table.AddColumn("2.5cm");
        
            var r = table.AddRow();
            r.Cells[0].AddParagraph("ID");
            r.Cells[1].AddParagraph("Название автомата");
            r.Cells[2].AddParagraph("Модель");
            r.Cells[3].AddParagraph("Компания владелец");
            r.Cells[4].AddParagraph("Модем");
            r.Cells[5].AddParagraph("Адрес/место");
            r.Cells[6].AddParagraph("В работе с");
            foreach (var machine in machines)
            {
                var row = table.AddRow();
                var p = row.Cells[0].AddParagraph(machine.Id.ToString());
                row.Cells[1].AddParagraph(machine.Name);
                row.Cells[2].AddParagraph(machine.ModelName);
                row.Cells[3].AddParagraph(machine.OwnerCompanyName);
                row.Cells[4].AddParagraph(machine.ModemNumber);
                row.Cells[5].AddParagraph($"{machine.Address} {machine.Place}");
                row.Cells[6].AddParagraph(machine.DateInstalled.ToShortDateString());
            }
            var renderer = new PdfDocumentRenderer()
            {
                Document = doc, 
            };
            renderer.RenderDocument();
            renderer.Save(pdfFilePath);   
        }
    }
    
}