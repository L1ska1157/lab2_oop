using CommunityToolkit.Mvvm.ComponentModel;

namespace Lab2.Models;

public partial class SearchModel : ObservableObject, ISearchCriteria
{
    [ObservableProperty] private List<string> _facultyOptions = new();
    [ObservableProperty] private List<string> _departmentOptions = new();
    [ObservableProperty] private List<string> _positionOptions = new();

    [ObservableProperty] private string? _fullName;
    [ObservableProperty] private string? _faculty;
    [ObservableProperty] private string? _department;
    [ObservableProperty] private string? _position;
    [ObservableProperty] private bool? _isFired;
    [ObservableProperty] private double? _salaryMin;
    [ObservableProperty] private double? _salaryMax;

    public void ClearFilters()
    {
        FullName = null;
        Faculty = null;
        Position = null;
        IsFired = null;
        SalaryMin = null;
        SalaryMax = null;
    }
}