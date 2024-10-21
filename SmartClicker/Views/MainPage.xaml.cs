using Microsoft.Maui.Controls;
using SmartClicker.ViewModels;
using SmartClicker.Services;
using System;

namespace SmartClicker.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
        }
    }
}
