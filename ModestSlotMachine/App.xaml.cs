﻿
namespace ModestSlotMachine
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            window.Width = 1280;
            window.Height = 1000;

            return window;
        }
    }
}