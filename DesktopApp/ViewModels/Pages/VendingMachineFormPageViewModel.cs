using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApp.Services;
using DesktopApp.Util;
using Models.Dto;
using Models.Entities;
using Task = System.Threading.Tasks.Task;

namespace DesktopApp.ViewModels.Pages;

public partial class VendingMachineFormPageViewModel : ViewModelBase
{
    private int _machineId;
    private HttpClient _httpClient;
    private MenuNavigationService _navigationService;
    [ObservableProperty] private AddUpdateVendingMachineDto _machine;
    [ObservableProperty] private string _formTitle = "Редактирование торгового автомата";
    [ObservableProperty] private List<Model> _models;
    [ObservableProperty] private List<Manufacturer> _manufacturers;
    [ObservableProperty] private List<ProductMatrix> _productMatrices;
    [ObservableProperty] private List<CheckValue<PaymentTypeDto>> _paymentTypes;
    [ObservableProperty] private List<MachineWorkStatus> _workStatuses;
    [ObservableProperty] private List<Company> _companies;
    
    public List<string> NotificationsTemplates => new() {"Не установлен", "Стандартный", "Нестандартный" };
    public List<string> Timezones => ["UTC", "UTC+1", "UTC+2", "UTC+3", "UTC+4", "UTC+5", 
        "UTC+6", "UTC+7", "UTC+8", "UTC+9", "UTC+10", "UTC+11", "UTC+12"];
    public List<string> CriticalValuesTemplates => new() {"Не установлен", "Стандартный", "Нестандартный" };
    public List<string> ServicePriorities => new() {"Высокий", "Средний", "Малый" };
    public List<string> WorkRegimes => new() {"Стандартный", "Нестандартный" };
    
    public VendingMachineFormPageViewModel(HttpClient client, MenuNavigationService navigationService)
    {
        _httpClient = client;
        _navigationService = navigationService;
    }

    private Task<HttpResult<AddUpdateVendingMachineDto>>? _machineTask;
    public async Task InitializeAsync(int machineId)
    {
        _machineId = machineId;
        _machineTask = _httpClient.SafeGetAsync<AddUpdateVendingMachineDto>("machines/" + machineId);
        var machineResponse = await _machineTask;
        if (machineResponse.TryGetResult(out var machine))
        {
            Machine = machine;
        }
        _machineTask = null;
    }
    
    [RelayCommand]
    public async Task LoadDataAsync()
    {
        var modelsTask = _httpClient.SafeGetAsync<List<Model>>("models");
        var manufacturersTask = _httpClient.SafeGetAsync<List<Manufacturer>>("manufacturers");
        var productMatricesTask = _httpClient.SafeGetAsync<List<ProductMatrix>>("product-matrices");
        var paymentTypesTask = _httpClient.SafeGetAsync<List<PaymentTypeDto>>("payment-types");
        var workStatusesTask = _httpClient.SafeGetAsync<List<MachineWorkStatus>>("work-statuses");
        var companiesTask = _httpClient.SafeGetAsync<List<Company>>("companies");

        var (models, modelsError) = await modelsTask;
        var (manufacturers, manufacturersError) = await manufacturersTask;
        var (productMatrices, productMatricesError) = await productMatricesTask;
        var (paymentTypes, paymentTypesError) = await paymentTypesTask;
        var (workStatuses, workStatusesError) = await workStatusesTask;
        var (companies, companiesError) = await companiesTask;

        if (_machineTask != null && !_machineTask.IsCompleted)
        {
            await _machineTask;
        }

        if (_machineId == 0)
        {
            Machine = new AddUpdateVendingMachineDto()
            {
                MachineAdditionalInformation = new MachineAdditionalInformationDto(),
                MachineTechnicalInformation = new MachineTechnicalInformationDto(),
                PaymentTypes = new List<PaymentTypeDto>(),
            };
            FormTitle = "Добавление торгового автомата";
        }
        
        if (models == null || manufacturers == null || productMatrices == null ||  paymentTypes == null 
            || Machine == null || workStatuses == null || companies == null)
        {
            await ShowError("Ошибка", "Во время загрузки данных произошла ошибка, попробуйте загрузить данные позже");
        }
        else
        {
            Models = models;
            Companies = companies;
            WorkStatuses = workStatuses;
            Manufacturers = manufacturers;
            ProductMatrices = productMatrices;
            PaymentTypes = paymentTypes.ConvertAll(x=> new CheckValue<PaymentTypeDto>()
            {
                Value = x,
                IsChecked = Machine.PaymentTypes.Any(t=>t.Id == x.Id)
            });
        }
    }

    public bool CheckMachineBeforeSave(AddUpdateVendingMachineDto machine)
    {
        var v = new Validator();
        v.Validate(string.IsNullOrWhiteSpace(machine.Address), "Не указан адрес ТА");
        v.Validate(string.IsNullOrWhiteSpace(machine.Place), "Не указано место ТА");
        v.Validate(string.IsNullOrWhiteSpace(machine.ModemNumber), "Не указан номер модема");
        v.Validate(string.IsNullOrWhiteSpace(machine.Name), "Не указано название");
        v.Validate(string.IsNullOrWhiteSpace(machine.MachineAdditionalInformation?.TemplateCriticalValues), "Не указан шаблон критических значений");
        v.Validate(string.IsNullOrWhiteSpace(machine.MachineAdditionalInformation?.TemplateNotifications), "Не указан шаблон уведомлений");
        v.Validate(string.IsNullOrWhiteSpace(machine.MachineAdditionalInformation?.WorkRegime), "Не указан рабочий режим");
        v.Validate(string.IsNullOrWhiteSpace(machine.MachineAdditionalInformation?.WorkTime), "Не указано время работы");
        v.Validate(machine.ModelId == 0, "Не указана модель");
        v.Validate(machine.OwnerCompanyId == 0, "Не указана компания владелец");
        v.Validate(machine.WorkStatusId == 0, "Не указан статус работы");
        v.Validate(machine.MachineTechnicalInformation?.CommandsAmount < 1, "Не указано количество команд > 0");
        v.Validate(machine.MachineTechnicalInformation?.DetailsAmount < 1, "Не указано количество деталей > 0");
        v.Validate(machine.ManufacturerId == 0, "Не указан производитель");
        v.Validate(machine.MachineAdditionalInformation?.ProductMatrixId == 0, "Не указана товарная матрица");
        v.Validate(!machine.PaymentTypes.Any(), "Не указан хотя бы 1 способ оплаты");
        return v.GetVerdict();
    }

    [RelayCommand]
    public async Task SaveDataAsync()
    {
        Machine.PaymentTypes = new List<PaymentTypeDto>();
        PaymentTypes.ForEach(x =>
        {
            if (x.IsChecked)
            {
                Machine.PaymentTypes.Add(x.Value);
            }
        });
        
        if (CheckMachineBeforeSave(Machine))
        {
            try
            {
                HttpResponseMessage response;
                if (Machine.Id == 0)
                {
                    response = await _httpClient.PostAsJsonAsync($"machines", Machine);
                }
                else
                {
                    response = await _httpClient.PutAsJsonAsync($"machines", Machine);
                }
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Произошла ошибка при сохранении, попробуйте позже");
                }
                await ShowInfo("Успешная операция",$"Информация о ТА сохранена");
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
        _navigationService.NavigateTo<VendingMachinesPageViewModel>();
    }

}