using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_ChatRoom.Views.Pages;

namespace WPF_ChatRoom.ViewModels
{
    class MainWindowVM : ViewModelBase
    {
        private UserControl _currentPage;

        public UserControl CurrentPage 
        {
            get => _currentPage;
            set { _currentPage = value;OnPropertyChanged(); }
        }
        public ICommand HomePage { get; }
        public ICommand SettingsPage { get; }
        public ICommand DataBasePage { get; }

        public MainWindowVM()
        {
            HomePage = new RelayCommand(_ => CurrentPage = new HomeView());
            SettingsPage = new RelayCommand(_ => CurrentPage = new SettingsView());
            DataBasePage = new RelayCommand(_ => CurrentPage = new DataBaseView());

            // 默认显示首页
            CurrentPage = new HomeView();
        }


    }
}
