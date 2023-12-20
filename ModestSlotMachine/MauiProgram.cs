using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using ModestSlotMachine.Areas.Slots.LibertyBellSlot.Services;
using ModestSlotMachine.Core.Audio;
using ModestSlotMachine.Core.Entities;
using ModestSlotMachine.Core.Repositories;
using ModestSlotMachine.Core.Services;
using ModestSlotMachine.Persistent;
using Plugin.Maui.Audio;
using System.Globalization;

namespace ModestSlotMachine
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

            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddSingleton<MsmAudioPlayer>();

            builder.Services.AddTransient<LibertyBellSlotService>();
            builder.Services.AddTransient<LibertyBellAudioService>();

            builder.Services.AddTransient<IUserDataRepository, LocalUserDataRepository>();
            builder.Services.AddTransient<UserDataService>();

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