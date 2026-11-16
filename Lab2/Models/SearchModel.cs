namespace Lab2.Models;

public class SearchModel : ISearchCriteria
{
    public List<string> FacultyOptions { get; } = new();
    public List<string> PositionOptions { get; } = new();
    
    public string? FullName { get; set; }
    public string? Faculty { get; set; }
    public string? Position { get; set; }
    public bool? IsFired { get; set; }
    public double? SalaryMin { get; set; }
    public double? SalaryMax { get; set; }

    void ClearFilters()
    {
        FullName = null;
        Faculty = null;
        Position = null;
        IsFired = null;
        SalaryMin = null;
        SalaryMax = null;
    }
}