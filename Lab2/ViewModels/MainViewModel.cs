using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lab2.Models;
using Lab2.Strategies;
using System.Xml;
using Lab2.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace Lab2.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly SearchContext _context;

    public SearchModel SearchFilters { get; } = new();
    public List<ISearchStrategy> Strategies { get; } =
        [new SaxSearchStrategy(), new DomSearchStrategy(), new LinqSearchStrategy()];
    public List<string> StatusOptions { get; } = ["Усі", "Працюють", "Звільнені"];
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    private FileResult? _selectedFile;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ExportCommand))]
    private List<Scientist> _results = new();
    
    [ObservableProperty] private string _output = "";
    [ObservableProperty] private int _statusIndex;
    [ObservableProperty] private ISearchStrategy _selectedStrategy;

    private bool HasFile() => SelectedFile is not null;
    private bool HasResults() => Results.Count > 0;

    private static Page CurrentPage => Application.Current!.Windows[0].Page!;

    [RelayCommand(CanExecute = nameof(HasFile))]
    private async Task SearchAsync()
    {
        try
        {
            await using Stream stream = await SelectedFile!.OpenReadAsync();
            Results = _context.Search(stream, SearchFilters);
            Output = Results.Count == 0
                ? "Нічого не знайдено"
                : string.Join("\n\n", Results);
        }
        catch (Exception ex)
        {
            await CurrentPage.DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    [RelayCommand(CanExecute = nameof(HasResults))]
    private async Task ExportAsync()
    {
        try
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, "result.html");

            using (Stream xslStream = await FileSystem.OpenAppPackageFileAsync("HtmlTemplate.xsl"))
            using (XmlReader xsl = XmlReader.Create(xslStream))
            using (FileStream file = File.Create(path))
            {
                new HtmlExporter().Export(Results, xsl, file);
            }

            await Launcher.OpenAsync(new OpenFileRequest("Результат пошуку", new ReadOnlyFile(path)));
        }
        catch (Exception ex)
        {
            CurrentPage.DisplayAlert("Помилка", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private void Clear()
    {
        SearchFilters.ClearFilters();
        StatusIndex = 0;
        Output = "";
        Results = new();
    }

    [RelayCommand]
    private async Task ExitAsync()
    {
        bool yes = await CurrentPage.DisplayAlert(
            "Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
        if (yes) Application.Current!.Quit();
    }

    public MainViewModel()
    {
        _selectedStrategy = Strategies[0];
        _context = new SearchContext(_selectedStrategy);
    }

    partial void OnSelectedStrategyChanged(ISearchStrategy value) => _context.Strategy = value;

    partial void OnStatusIndexChanged(int value) =>
        SearchFilters.IsFired = value switch { 1 => false, 2 => true, _ => null };

    [RelayCommand]
    private async Task PickFileAsync()
    {
        var file = await FilePicker.Default.PickAsync();
        if (file is null) return;

        try
        {
            await using var fileStream = await file.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);

            memoryStream.Position = 0;
            var faculties = _context.GetValues(memoryStream, "faculty");

            memoryStream.Position = 0;
            var positions = _context.GetValues(memoryStream, "position");

            SearchFilters.ClearFilters();
            StatusIndex = 0;
            Output = "";
            SelectedFile = file;
            SearchFilters.FacultyOptions = faculties;
            SearchFilters.PositionOptions = positions;
        }
        catch (Exception ex)
        {
            await CurrentPage.DisplayAlert("Помилка", ex.Message, "OK");
        }
    }
}