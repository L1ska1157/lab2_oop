using System.Globalization;
using System.Xml.Linq;
using Lab2.Models;

namespace Lab2.Strategies;

public class LinqSearchStrategy : ISearchStrategy
{
    public string Name => "LINQ";

    public List<Scientist> Search(string filePath, ISearchCriteria criteria)
    {
        XDocument doc = XDocument.Load(filePath);
        var elements = doc.Root?.Elements("scientist") ?? Enumerable.Empty<XElement>();
        
        if (!string.IsNullOrWhiteSpace(criteria.Faculty))
        {
            elements = elements.Where(el => (string?)el.Attribute("faculty") == criteria.Faculty);
        }
        
        if (!string.IsNullOrWhiteSpace(criteria.Position))
        {
            elements = elements.Where(el => (string?)el.Attribute("position") == criteria.Position);
        }

        if (!string.IsNullOrWhiteSpace(criteria.FullName))
        {
            elements = elements.Where(el =>
            {
                var name = (string?)el.Element("fullName");
                return name != null && name.Contains(criteria.FullName);
            });
        }

        if (criteria.IsFired.HasValue)
        {
            elements = elements.Where(el => (el.Attribute("fireDate") != null) == criteria.IsFired.Value);
        }

        if (criteria.SalaryMin.HasValue)
        {
            elements = elements.Where(el =>
                double.Parse((string)el.Attribute("salary")!, CultureInfo.InvariantCulture) >= criteria.SalaryMin.Value);
        }
        
        if (criteria.SalaryMax.HasValue)
        {
            elements = elements.Where(el =>
                double.Parse((string)el.Attribute("salary")!) <= criteria.SalaryMax.Value);
        }

        return elements.Select(el =>
        {
            return new Scientist(
                fullName: (string)el.Element("fullName")!,
                faculty: (string)el.Attribute("faculty")!,
                department: (string)el.Attribute("department")!,
                position: (string)el.Attribute("position")!,
                salary: double.Parse((string)el.Attribute("salary")!, CultureInfo.InvariantCulture),
                hireDate: (string)el.Attribute("hireDate")!,
                fireDate: (string?)el.Attribute("fireDate")
            );
        }).ToList();
    }

    public List<string> GetValues(string filePath, string attributeName)
    {
        var doc = XDocument.Load(filePath);
        var elements = doc.Root?.Elements("scientist") ?? Enumerable.Empty<XElement>();

        HashSet<string> options = new();
        
        foreach (XElement el in elements)
        {
            string? attrValue = el.Attribute(attributeName)?.Value;
            
            if (!string.IsNullOrWhiteSpace(attrValue)) options.Add(attrValue);
        }
        return options.Order().ToList();
    }
}