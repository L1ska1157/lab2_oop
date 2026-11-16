using System.Globalization;
using System.Xml;
using Lab2.Models;

namespace Lab2.Strategies;

public class DomSearchStrategy : ISearchStrategy
{
    public string Name => "DOM";

    public List<Scientist> Search(string filePath, ISearchCriteria criteria)
    {
        List<Scientist> results = new();

        XmlDocument doc = new();
        doc.Load(filePath);

        XmlNodeList? nodes = doc.SelectNodes("/scientists/scientist");
        if (nodes == null) return results;

        foreach (XmlElement el in nodes)
        {
            string faculty = el.GetAttribute("faculty");
            string department = el.GetAttribute("department");
            string position = el.GetAttribute("position");
            double salary = double.Parse(el.GetAttribute("salary"), CultureInfo.InvariantCulture);
            string hireDate = el.GetAttribute("hireDate");
            string? fireDate = el.HasAttribute("fireDate") ? el.GetAttribute("fireDate") : null;
            string fullName = el["fullName"]?.InnerText ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(criteria.Faculty) && faculty != criteria.Faculty)
                continue;
            
            if (!string.IsNullOrWhiteSpace(criteria.Position) && position != criteria.Position)
                continue;

            if (criteria.SalaryMin != null && salary > criteria.SalaryMax)
                continue;
            
            if (criteria.SalaryMin != null && salary < criteria.SalaryMin)
                continue;
            
            if (criteria.IsFired.HasValue && el.HasAttribute("fireDate") != criteria.IsFired.Value)
                continue;
            
            if (!string.IsNullOrWhiteSpace(criteria.FullName) && !fullName.Contains(criteria.FullName))
                continue;

            results.Add(new Scientist(
                fullName: fullName,
                faculty: faculty,
                department: department,
                position: position,
                salary: salary,
                hireDate: hireDate,
                fireDate: fireDate
            ));
        }

        return results;
    }

    public List<string> GetValues(string filePath, string attributeName)
    {
        HashSet<string> options = new();

        XmlDocument doc = new();
        doc.Load(filePath);

        XmlNodeList? nodes = doc.SelectNodes("/scientists/scientist");
        if (nodes == null) return options.ToList();

        foreach (XmlElement el in nodes)
        {
            string attr =  el.GetAttribute(attributeName);
            if (attr.IsWhiteSpace()) 
                continue;
            options.Add(attr);
        }
        return options.Order().ToList();
    }
}