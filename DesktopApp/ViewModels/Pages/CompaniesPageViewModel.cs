

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using DesktopApp.Services;
using DesktopApp.Util;
using MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using Models.Domain;
using Models.Dto;
using Models.Entities;
using PdfSharp.Fonts;
using PdfSharp.Snippets.Font;
using Colors = Avalonia.Media.Colors;
using Task = System.Threading.Tasks.Task;

namespace DesktopApp.ViewModels.Pages;

public partial class CompaniesPageViewModel : ViewModelBase
{
    private HttpClient _httpClient;
    private MenuNavigationService _navigationService;

    public CompaniesPageViewModel(HttpClient client, MenuNavigationService navigationService)
    {
        _navigationService = navigationService;
        _httpClient = client;
    }
    [ObservableProperty] private List<CompanyDto> _companies;
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

    public async Task<List<CompanyDto>> LoadAllCompaniesForExportAsync()
    {
        var companiesResponse = await _httpClient.SafeGetAsync<List<CompanyDto>>($"companies/export");
        if (companiesResponse.TryGetResult(out var companies))
        {
            return companies;
        }
        else
        {
            await ShowError("Ошибка", companiesResponse.Exception.Message);
        }
        return new();
    }
    
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        var query = $"companies/pages?pageNumber={(_dataChanged? 1 : PageNumber)}&pageLimit={PageLimit}";
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            query += $"&searchText={Uri.EscapeDataString(SearchText)}";
        }
        var companiesResponse = await _httpClient.SafeGetAsync<CompaniesAndCount>(query);

        if (companiesResponse.TryGetResult(out var companiesAndCount))
        {
            Companies = companiesAndCount.Companies;
            TotalCount = companiesAndCount.TotalCount;
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
            var firstRow = (PageNumber - 1) * PageLimit + (Companies.Count == 0? 0 : 1);
            TotalBlockText = $"Записи с {firstRow} по {firstRow + Companies.Count - 1}";
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
    private async Task DeleteAsync(int companyId)
    {
        if (await Confirm("Подтверждение операции","Вы уверены, что хотите удалить эту компанию?"))
        {
            try
            {
                var res = await _httpClient.DeleteAsync($"companies/{companyId}");
                if (!res.IsSuccessStatusCode)
                {
                    throw new Exception("Эту компанию нельзя удалить, ее данные используются для документов");
                }
                await ShowInfo("Успешная операция",$"Компания успешно удалена");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                await ShowError("Ошибка",$"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }
    
    [RelayCommand]
    public void Add()
    {
        _navigationService.NavigateTo<CompanyFormPageViewModel>();
    }
    [RelayCommand]
    public async Task Edit(int companyId)
    {
        _navigationService.NavigateTo<CompanyFormPageViewModel>();
        if (_navigationService.CurrentViewModel is CompanyFormPageViewModel formViewModel)
        {
            await formViewModel.InitializeAsync(companyId);
        }
    }
    
    [RelayCommand]
    public async Task CsvExport(string csvFilePath)
    {
        var companies = await LoadAllCompaniesForExportAsync();
        if (companies.Any())
        {
            using (var file = System.IO.File.Create(csvFilePath))
            {
                using var writer = new StreamWriter(file, Encoding.UTF8);
                using (var csvParser = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csvParser.Context.RegisterClassMap<CompanyCsvMap>();
                    await csvParser.WriteRecordsAsync(companies);
                }
            }
        }
    }

    [RelayCommand]
    public async Task HtmlExport(string htmlFilePath)
    {
        var companies = await LoadAllCompaniesForExportAsync();
        if (companies.Any())
        {
            var result = new StringBuilder();
            result.AppendLine(
                "<!DOCTYPE html>\n<html lang=\"en\">\n<head>\n<meta charset=\"UTF-8\">\n<title>Список компаний</title>\n</head>\n<body>\n<table>\n<thead>\n<tr>\n<th>Название компании</th>\n<th>Название вышестоящей компании</th>\n<th>Адрес</th>\n<th>Контакты</th>\n<th>В работе с</th>\n</tr>\n</thead>\n<tbody>");
            foreach (var company in companies)
            {
                result.Append("<tr>");
                result.Append("<td>" + company.Name + "</td>");
                result.Append("<td>" + company.UpperCompanyName + "</td>");
                result.Append("<td>" + company.Address + "</td>");
                result.Append("<td>" + company.Contacts + "</td>");
                result.Append("<td>" + company.DateWorkStarted + "</td>");
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
        var companies = await LoadAllCompaniesForExportAsync();
        if (companies.Any())
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
            
            table.AddColumn();
            table.AddColumn();
            table.AddColumn();
            table.AddColumn("3cm");
            table.AddColumn();

            var r = table.AddRow();
            r.Cells[0].AddParagraph("Название компании");
            r.Cells[1].AddParagraph("Название вышестоящей компании");
            r.Cells[2].AddParagraph("Адрес");
            r.Cells[3].AddParagraph("Контакты");
            r.Cells[4].AddParagraph("В работе с");
            foreach (var company in companies)
            {
                var row = table.AddRow();
                row.Cells[0].AddParagraph(company.Name);
                row.Cells[1].AddParagraph(company.UpperCompanyName ?? "");
                row.Cells[2].AddParagraph(company.Address);
                row.Cells[3].AddParagraph(company.Contacts);
                row.Cells[4].AddParagraph(company.DateWorkStarted.ToShortDateString());
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