using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Lab2.Models;
using Lab2.Strategies;

var path = "/Users/l1ska/Developer/OOP/Lab2/Lab2/scientists_test.xml"; 
var strategy = new DomSearchStrategy();

var faculties = strategy.GetValues(path, "faculty");
Console.WriteLine("Факультети: " + string.Join(", ", faculties));

var all = strategy.Search(path, new SearchModel());
Console.WriteLine($"Всього знайдено: {all.Count}");

var filtered = strategy.Search(path, new SearchModel
{
    Faculty = "Факультет_1",
    IsFired = false,
    FullName = "Слово"
});

Console.WriteLine($"Відфільтровано: {filtered.Count}");
foreach (var s in filtered)
{
    Console.WriteLine(s);
    Console.WriteLine("---");
}

// --- Тест XSL ---
var dir = Path.GetDirectoryName(path)!;
var xslPath = "/Users/l1ska/Developer/OOP/Lab2/Lab2/Resources/HtmlTemplate.xsl";

ExportToHtml(all, xslPath, Path.Combine(dir, "result_all.html"));
ExportToHtml(filtered, xslPath, Path.Combine(dir, "result_filtered.html"));

Console.WriteLine("HTML збережено");

static void ExportToHtml(List<Scientist> scientists, string xslPath, string htmlPath)
{
    var inv = CultureInfo.InvariantCulture;

    var doc = new XDocument(
        new XElement("scientists",
            scientists.Select(sc => new XElement("scientist",
                new XAttribute("faculty", sc.Faculty),
                new XAttribute("department", sc.Department),
                new XAttribute("position", sc.Position),
                new XAttribute("salary", sc.Salary.ToString(inv)),
                new XAttribute("hireDate", sc.HireDate.ToString("yyyy-MM-dd", inv)),
                sc.FireDate is { } fd ? new XAttribute("fireDate", fd.ToString("yyyy-MM-dd", inv)) : null,
                new XElement("fullName", sc.FullName)))));

    var xslt = new XslCompiledTransform();
    xslt.Load(xslPath);

    using var writer = XmlWriter.Create(htmlPath, xslt.OutputSettings);
    xslt.Transform(doc.CreateReader(), writer);
}