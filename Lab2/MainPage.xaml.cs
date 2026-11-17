using Lab2.ViewModels;
using Microsoft.Maui.Controls;

namespace Lab2;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainViewModel();
    }
}