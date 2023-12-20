using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
#if WINDOWS
using Microsoft.UI.Windowing;
#endif

namespace ModestSlotMachine.Util
{
    internal static class WindowMode
    {
#if WINDOWS
        private static AppWindow GetAppWindow(MauiWinUIWindow window)
        {
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(window);
            var id = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(handle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(id);
            return appWindow;
        }
#endif

        public static void SetFullScreenWindowMode()
        {
#if WINDOWS
            var window = Application.Current.Windows.First().Handler.PlatformView as MauiWinUIWindow;
            var appWindow = GetAppWindow(window);
            window.ExtendsContentIntoTitleBar = false;
            appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
            appWindow.TitleBar.ExtendsContentIntoTitleBar = false;
#endif
        }

        public static void SetWindowedScreenWindowMode()
        {
#if WINDOWS
            var window = Application.Current.Windows.First().Handler.PlatformView as MauiWinUIWindow;
            var appWindow = GetAppWindow(window);
            appWindow.SetPresenter(AppWindowPresenterKind.Default);            
            appWindow.TitleBar.ExtendsContentIntoTitleBar = true;

            //RestartApp();
#endif
        }

        private static void RestartApp()
        {
            var filename = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(filename);

            Application.Current.Quit();
        }
    }
}
