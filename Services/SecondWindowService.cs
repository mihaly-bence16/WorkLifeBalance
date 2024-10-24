﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;
using WorkLifeBalance.Interfaces;
using WorkLifeBalance.ViewModels;

namespace WorkLifeBalance.Services
{
    public partial class SecondWindowService : ObservableObject, ISecondWindowService
    {
        private readonly INavigationService navigation;

        [ObservableProperty]
        private SecondWindowPageVMBase? loadedPage;

        private SecondWindowPageVMBase? activeSecondWindowPage;

        public SecondWindowService(INavigationService navigation)
        {
            this.navigation = navigation;
        }

        private void CloseWindow()
        {
            _ = Task.Run(ClearPage);
            //secondWindowView.Hide();
        }

        private async Task ClearPage()
        {
            if(activeSecondWindowPage != null)
            {
                await activeSecondWindowPage.OnPageClosingAsync();
                activeSecondWindowPage = null;
            }
        }

        public async Task OpenWindowWith<T>(object? args = null) where T : SecondWindowPageVMBase 
        {
            _ = Task.Run(ClearPage);

            SecondWindowPageVMBase loading = (SecondWindowPageVMBase)navigation.NavigateTo<LoadingPageVM>();

            LoadedPage = loading;
            //secondWindowView.Show();

            activeSecondWindowPage = (SecondWindowPageVMBase)navigation.NavigateTo<T>();

            await Task.Delay(300);

            _ = Task.Run(async () =>
            {
                await activeSecondWindowPage.OnPageOppeningAsync(args);

                App.Current.Dispatcher.Invoke(() =>
                {
                    LoadedPage = activeSecondWindowPage;
                });
            });
        }
    }
}
