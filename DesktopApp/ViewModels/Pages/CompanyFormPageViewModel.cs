using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApp.Services;
using DesktopApp.Util;
using Models.Dto;
using Models.Entities;
using Task = System.Threading.Tasks.Task;


namespace DesktopApp.ViewModels.Pages;

public partial class CompanyFormPageViewModel : ViewModelBase
{
    private int _companyId;
    private HttpClient _httpClient;
    private MenuNavigationService _navigationService;
    public CompanyFormPageViewModel(HttpClient client, MenuNavigationService navigationService)
    {
        _httpClient = client;
        _navigationService = navigationService;
    }
    
    [ObservableProperty] private string _formTitle = "Редактирование компании";
    [ObservableProperty] private List<Company> _companies;
    [ObservableProperty] private CompanyDto _company; 
    
    private Task<HttpResult<CompanyDto>>? _companyTask;
    
    public async Task InitializeAsync(int companyId)
    {
        _companyId = companyId;
        _companyTask = _httpClient.SafeGetAsync<CompanyDto>("companies/" + companyId);
        var companyResponse = await _companyTask;
        if (companyResponse.TryGetResult(out var company))
        {
            Company = company;
        }
        _companyTask = null;
    }
    
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        var companiesTask = _httpClient.SafeGetAsync<List<Company>>("companies");
        
        var (companies, companiesError) = await companiesTask;

        if (_companyTask != null && !_companyTask.IsCompleted)
        {
            await _companyTask;
        }

        if (_companyId == 0)
        {
            Company = new CompanyDto();
            FormTitle = "Добавление компании";
        }
        
        if (Company == null || companies == null)
        {
            await ShowError("Ошибка", "Во время загрузки данных произошла ошибка, попробуйте загрузить данные позже");
        }
        else
        {
            Companies = companies;
        }
    }

    public bool CheckCompanyBeforeSave(CompanyDto company)
    {
        var v = new Validator();
        v.Validate(string.IsNullOrWhiteSpace(company.Name), "Не указан адрес ТА");
        v.Validate(string.IsNullOrWhiteSpace(company.Address), "Не указано место ТА");
        v.Validate(string.IsNullOrWhiteSpace(company.Contacts), "Не указан номер модема");
        return v.GetVerdict();
    }
    
    [RelayCommand]
    public async Task SaveDataAsync()
    {
        if (CheckCompanyBeforeSave(Company))
        {
            try
            {
                HttpResponseMessage response;
                if (Company.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync($"companies", Company);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"companies", Company);
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Произошла ошибка при сохранении, попробуйте позже");
                }
                await ShowInfo("Успешная операция",$"Информация о компании сохранена");
                CloseForm();
            }
            catch (Exception ex)
            {
                await ShowError("Ошибка",$"{ex.Message}\n{ex.InnerException?.Message}");
            }
        }
    }

    [RelayCommand]
    public void CloseForm()
    {
        _navigationService.NavigateTo<CompaniesPageViewModel>();
    }
}