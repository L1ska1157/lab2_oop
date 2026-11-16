using System.Data;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Lab2.Models;
using Lab2.Strategies;
using Lab2.Services;

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
using XmlReader xsl = XmlReader.Create(xslPath);
using FileStream file = File.Create(Path.Combine(dir, "result.html"));
new HtmlExporter().Export(filtered, xsl, file);


Console.WriteLine("HTML збережено");