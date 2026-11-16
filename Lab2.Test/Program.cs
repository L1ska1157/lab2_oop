using Lab2.Models;
using Lab2.Strategies;

var path = "/Users/l1ska/Developer/OOP/Lab2/Lab2/scientists_test.xml"; // вкажи свій шлях до файлу
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