using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using DesktopApp.Views;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace DesktopApp.Util;

public static class MessageHelper
{
    public static async Task ShowInfo(string title, string message)
    {
        await MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok, Icon.Info).ShowAsync();
    }
    public static async Task ShowError(string title, string message)
    {
        await MessageBoxManager.GetMessageBoxStandard(title, message, ButtonEnum.Ok, Icon.Error).ShowAsync();
    }
    public static async Task<bool> Confirm(string title, string message)
    {
        return await MessageBoxManager
            .GetMessageBoxStandard(title, message, ButtonEnum.YesNo, Icon.Question)
            .ShowAsync() == ButtonResult.Yes;
    }
    
    public static async Task<IStorageFile?> GetPathToSaveAsync(string fileExtension, string suggestedFileName)
    {
        try
        {
            var desktop = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
            var mainWindow = desktop?.MainWindow as MainWindow;
            var storage = mainWindow?.StorageProvider;
            var selectedFilePath = await storage.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                DefaultExtension = fileExtension,
                SuggestedFileName = suggestedFileName,
                FileTypeChoices = [new FilePickerFileType(fileExtension)
                {
                    Patterns = [$"*{fileExtension}"]
                }]
            });
            return selectedFilePath;
        }
        catch (Exception ex)
        {
            await ShowError("Ошибка", ex.Message);
            return null;
        }
    }
    
    public static async Task<string?> AskUserPathWithDateToSave(string fileExtension, string suggestedPrefix)
    {
        var date = DateOnly.FromDateTime(DateTime.Now);
        var suggestedName = $"{suggestedPrefix} {date.ToString("MM dd yyyy")}.{fileExtension}";
        var userSelectedPath= await GetPathToSaveAsync(fileExtension,suggestedName);
        return userSelectedPath?.Path?.LocalPath;
    }
}