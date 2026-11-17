using System.Globalization;
using System.Xml;
using Lab2.Models;

namespace Lab2.Strategies;

public class SaxSearchStrategy : ISearchStrategy
{
    public string Name => "SAX";

    public List<Scientist> Search(Stream fileStream, ISearchCriteria criteria)
    {
        List<Scientist> results = new();

        using XmlReader reader = XmlReader.Create(fileStream);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "scientist")
            {
                string faculty = reader.GetAttribute("faculty")!;
                string department = reader.GetAttribute("department")!;
                string position = reader.GetAttribute("position")!;
                double salary = double.Parse(reader.GetAttribute("salary") ?? "0", CultureInfo.InvariantCulture);
                string hireDate = reader.GetAttribute("hireDate")!;
                string? fireDate = reader.GetAttribute("fireDate");

                if (!string.IsNullOrWhiteSpace(criteria.Faculty) && faculty != criteria.Faculty)
                {
                    continue;
                }


                if (!string.IsNullOrWhiteSpace(criteria.Position) && position != criteria.Position)
                {
                    continue;
                }

                if (criteria.SalaryMax != null && salary > criteria.SalaryMax)
                {
                    continue;
                }

                if (criteria.SalaryMin != null && salary < criteria.SalaryMin)
                {
                    continue;
                }
                
                if (criteria.IsFired.HasValue && (fireDate != null) != criteria.IsFired.Value)
                {
                    continue;
                }

                if (!reader.ReadToDescendant("fullName"))
                    continue;

                string fullName = reader.ReadElementContentAsString();

                if (!string.IsNullOrWhiteSpace(criteria.FullName) && !fullName.Contains(criteria.FullName, StringComparison.OrdinalIgnoreCase))
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
        }

        return results;
    }

    public List<string> GetValues(Stream fileStream, string attributeName)
    {
        HashSet<string> options = new();
        
        using XmlReader reader = XmlReader.Create(fileStream);
        while (reader.Read())
        {
            if (reader.NodeType == XmlNodeType.Element && reader.Name == "scientist")
            {
                string attr = reader.GetAttribute(attributeName)!;
                if (!string.IsNullOrWhiteSpace(attr)) options.Add(attr);
            }
        }
        return options.Order().ToList();
    }
}