using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DesktopApp.Services;
using DesktopApp.Views;
using Models.Dto;

namespace DesktopApp.ViewModels.Pages;

public partial class AuthPageViewModel(HttpClient httpClient, MainWindowNavigationService navigationService, AuthService authService) : ViewModelBase
{
    [ObservableProperty] private string _email = "";
    [ObservableProperty] private string _password = "";

    [RelayCommand]
    public async System.Threading.Tasks.Task SignInAsync()
    {
        var authorizationRequest = new AuthorizationRequest()
        {
            Email = Email,
            Password = Password
        };
        try
        {
            var response = await httpClient.PostAsJsonAsync("sign-in", authorizationRequest);
            if (response.IsSuccessStatusCode)
            {
                AuthorizationResponse authResponse = await response.Content.ReadFromJsonAsync<AuthorizationResponse>();
                if (authResponse.Role.Id != 1)
                {
                    await ShowError("Ошибка", "У данного аккаунта нет прав доступа к кабинету франчайзера");
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Token);
                    authService.PutUserData(authResponse);
                    navigationService.NavigateTo<MenuAndContentPageViewModel>("ООО Торговые автоматы");
                }
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new AuthenticationException(errorText);
            }
        }
        catch (Exception ex)
        {
            await ShowError("Ошибка", $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
    
}