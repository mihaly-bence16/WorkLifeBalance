﻿using System;
using System.Threading.Tasks;
using WorkLifeBalance.Interfaces;
using WorkLifeBalance.ViewModels;

namespace WorkLifeBalance.Services
{
    public class SecondWindowService : ISecondWindowService
    {
        private readonly SecondWindowVM secondWindowVm;
        private readonly SecondWindow secondWindowView;
        private readonly INavigationService navigation;

        public SecondWindowService(SecondWindowVM secondWindowVm, SecondWindow secondWindowView, INavigationService navigation)
        {
            this.secondWindowVm = secondWindowVm;
            this.secondWindowView = secondWindowView;
            this.navigation = navigation;
            secondWindowVm.OnWindowClosing += ClearWindow;
        }

        private async Task ClearWindow()
        {
            SecondWindowPageVMBase activeModel = (SecondWindowPageVMBase)navigation.ActiveView;
            await activeModel.OnPageClosingAsync();
            secondWindowView.Hide();
        }

        public async Task OpenWindowWith<T>(object? args = null) where T : SecondWindowPageVMBase 
        {
            Console.WriteLine($"WindowRequested of type {typeof(T)}");
            navigation.NavigateTo<T>();
            SecondWindowPageVMBase activeModel = (SecondWindowPageVMBase)navigation.ActiveView;
            secondWindowVm.Width = (int)activeModel.RequiredWindowSize.X;
            secondWindowVm.Height = (int)activeModel.RequiredWindowSize.Y;
            secondWindowVm.PageName = activeModel.WindowPageName;
            secondWindowVm.ActivePage = activeModel;
            await activeModel.OnPageOppeningAsync(args);
            secondWindowView.Show();
        }
    }
}
