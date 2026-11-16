using System.Globalization;
using System.Text;

namespace Lab2.Models;

public class Scientist(
    string fullName,
    string faculty,
    string department,
    string position,
    double salary,
    string hireDate,
    string? fireDate
    )
{
    public string FullName { get; } = fullName;
    public string Faculty { get; } = faculty;
    public string Department { get; } = department;
    public string Position { get; } = position;
    public double Salary { get; } = salary;

    public DateOnly HireDate { get; } = DateOnly.ParseExact(hireDate, "yyyy-MM-dd", null);

    public DateOnly? FireDate { get; } = !string.IsNullOrWhiteSpace(fireDate)
        ? DateOnly.ParseExact(fireDate, "yyyy-MM-dd", null)
        : null;

    public bool IsFired { get; } = !string.IsNullOrWhiteSpace(fireDate);

    public override string ToString()
    {
        var ukCulture = CultureInfo.GetCultureInfo("uk-UA");
        var sb = new StringBuilder();
        
        sb.AppendLine($"ПІБ: {FullName}");
        sb.AppendLine($"Факультет: {Faculty} | Кафедра: {Department}");
        sb.AppendLine($"Посада: {Position} | Оклад: {Salary.ToString("N2", ukCulture)} грн");
        sb.AppendLine($"Прийнятий: {HireDate.ToString("dd.MM.yyyy", ukCulture)}");
        
        if (IsFired)
        {
            sb.AppendLine($"Звільнений: {FireDate!.Value.ToString("dd.MM.yyyy", ukCulture)}");
        }
        else
        {
            sb.AppendLine("Статус: Працює");
        }

        return sb.ToString().TrimEnd();
    }
}