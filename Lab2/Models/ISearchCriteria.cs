namespace Lab2.Models;

public interface ISearchCriteria
{
    string? FullName { get; }
    string? Faculty { get; }
    string? Position { get; }
    bool? IsFired { get; }
    double? SalaryMin { get; }
    double? SalaryMax { get; }
}