using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using ModestySlotMachine.Areas.Slots.LibertyBellSlot.Services;
using ModestySlotMachine.Core.Entities;
using ModestySlotMachine.Core.Repositories;
using ModestySlotMachine.Core.Services;
using ModestySlotMachine.Persistent;
using System.Globalization;

namespace ModestySlotMachine
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<LibertyBellSlotService>();
            builder.Services.AddTransient<LibertyBellAudioService>();

            builder.Services.AddTransient<IUserDataRepository, LocalUserDataRepository>();
            builder.Services.AddTransient<UserDataService>();

#if WINDOWS
            //builder.ConfigureLifecycleEvents(events =>  
            //{  
            //    events.AddWindows(wndLifeCycleBuilder =>  
            //    {  
            //        wndLifeCycleBuilder.OnWindowCreated(window =>  
            //        {  
            //            window.ExtendsContentIntoTitleBar = false;  
            //            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            //            Microsoft.UI.WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);  
            //            var _appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(myWndId);  
            //            _appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);                  
            //        });  
            //    });  
            //});
            //builder.ConfigureLifecycleEvents(events =>
            //{
            //    events.AddWindows(wndLifeCycleBuilder =>
            //    {
            //        wndLifeCycleBuilder.OnWindowCreated(window =>
            //        {
            //            window.ExtendsContentIntoTitleBar = false;
            //            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            //            Microsoft.UI.WindowId myWndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            //            var _appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(myWndId);
            //            _appWindow.SetPresenter(Microsoft.UI.Windowing.AppWindowPresenterKind.FullScreen);
            //        });
            //    });
            //});
#endif
             InitApp(builder.Services.BuildServiceProvider()).ConfigureAwait(false);

            return builder.Build();
        }

        public static async Task InitApp(ServiceProvider serviceProvider)
        {
            var userDataService = serviceProvider.GetRequiredService<UserDataService>();
            UserData userData = await userDataService.GetUserDataAsync();

            SetLocale(userData.UserSettings.Selectedlanguage);
        }

        public static void SetLocale(string locale)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = new System.Globalization.CultureInfo(locale);
        }
    }
}