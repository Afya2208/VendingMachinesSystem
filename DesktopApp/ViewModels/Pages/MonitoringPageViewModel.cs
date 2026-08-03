using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Models.Domain;
using Models.Entities;
using Task = System.Threading.Tasks.Task;

namespace DesktopApp.ViewModels.Pages;

public partial class MonitoringPageViewModel : ViewModelBase
{
    private HttpClient _httpClient;
    public MonitoringPageViewModel(HttpClient client)
    {
        _httpClient =  client;
    }
    
    [ObservableProperty] private List<MachineDomain> _machines;
    [ObservableProperty] private int _selectedStatusIdFilter;
    [ObservableProperty] private int _selectedConnectionTypeIdFilter;
    [ObservableProperty] private int _selectedAdditionalStatusIdFilter;
    [ObservableProperty] private DateTime _time = DateTime.Now;
    [ObservableProperty] private string _resultText;
    [ObservableProperty] private int _selectedSort;

    async void CreatePushWindows()
    {
        List<(string, int)> messages = new List<(string, int)>();
        foreach (var machine in Machines)
        {
            /*
            if (machine.MachineLoad.Minimal == 0 || machine.MachineLoad.Total == 0)
            {
                messages.Add(("Ошибка: закончились товары!", 3));
            }
            if (machine.MachineProvider.Ping == 2)
            {
                messages.Add(("Внимание: низкий уровень связи", 2));
            }
            if (machine.MachineProvider.Ping == 1)
            {
                messages.Add(("Ошибка: крайне низкий уровень связи", 3));
            }
            //todo
            // сделать нормальный анализ последних покупок и других нейтральных событий
           
            if (machine.Statuses.Any(x=>x.Id == 4))
            {
                messages.Add(("Ошибка: закончилась наличка в автомате", 3));
            }
            if (machine.Statuses.Any(x=>x.Id == 5))
            {
                messages.Add(("Ошибка: произошла поломка!", 3));
            }
            */
        }
        messages =  messages.OrderByDescending(x => x.Item2).ToList();
        foreach (var message in messages)
        {
            // todo
            // добавить отправку сообщений в БД через API
            /*
            PushWindow pushWindow = new PushWindow()
            {
                Position = new PixelPoint(_mainWindow.Position.X + (int)_mainWindow.Bounds.Width-350, 
                    _mainWindow.Position.Y + (int)_mainWindow.Bounds.Height-150)
            };
            pushWindow.DataContext = new PushWindowViewModel(message.Item1, message.Item2, pushWindow);
            await pushWindow.ShowDialog(_mainWindow);
            */
        }
    }
    
    [RelayCommand]
    private async System.Threading.Tasks.Task Filter()
    {
        await ReadData(SelectedStatusIdFilter, SelectedConnectionTypeIdFilter, SelectedAdditionalStatusIdFilter);
        if (SelectedSort == 0)
        {
            Machines = Machines.OrderBy(x => x.Machine.WorkStatusId).ToList();
        }
        for (int i = 0; i < Machines.Count; i++)
        {
            //Machines[i].Order = i+1;
        }
        ResultText = Machines.Count==0?"Нет активных торговых автоматов, соответствующих выбранным фильтрам":
            $"Итого автоматов: {Machines.Count}({Machines.Count(x=>x.Machine.WorkStatusId==1)}/" +
            $"{Machines.Count(x=>x.Machine.WorkStatusId==2)}/" +
            $"{Machines.Count(x=>x.Machine.WorkStatusId==3)}) " +
            $"Денег в автоматах: {Machines.Sum(x=> x.Machine.IncomeCash)} р. + {Machines.Sum(x=>x.Machine.ChangeCash)} р. (сдача)";
    }
    
    public async System.Threading.Tasks.Task ReadData(int statusId = 0, int connectionTypeId = 0, int additionalStatusId = 0)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<List<VendingMachine>>("/machines");
            if (response != null)
            {
                if (statusId != 0)
                    response = response.FindAll(x => x.WorkStatusId == statusId);
                if (connectionTypeId != 0)
                    response = response.FindAll(x => x.ConnectionTypes.Any(x=>x.Id == connectionTypeId));
                if (additionalStatusId != 0)
                    response = response.FindAll(x => x.Statuses.Any(u=>u.Id==additionalStatusId+3));
                Machines = new List<MachineDomain>();
            }
        }
        catch (Exception ex)
        {
            
        }
       
       foreach (var machine in Machines)
       {
            
            /*
            machine.Machine.Statuses = await _client.GetFromJsonAsync<List<Status>>("gen/statuses");
            machine.Machine.MachineMoney = await _client.GetFromJsonAsync<MachineMoney>("gen/money");
            machine.Machine.MachineLoad = await _client.GetFromJsonAsync<MachineLoad>("gen/load");
            machine.Machine.MachineProvider = await _client.GetFromJsonAsync<MachineProvider>($"gen/ping/{machine.Machine.Id}");
            */
       }
    }

    [RelayCommand]
    public void ChoseRadioButton()
    {
        
    }

    [RelayCommand]
    public async Task ApplyFilterAndSort()
    {
        
    }
    [RelayCommand]
    public async Task Clean()
    {
        
    }

    [RelayCommand]
    public async System.Threading.Tasks.Task CleanRadioButtons()
    {
        //todo
        // сделать обнуление полей
        await Filter();
    }

}