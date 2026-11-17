using System.Diagnostics;
using Lab2.Models;

namespace Lab2.Strategies;

public class SearchContext(ISearchStrategy searchStrategy)
{
    public ISearchStrategy Strategy { get; set; } = searchStrategy;
    
    public List<Scientist> Search(Stream fileStream, ISearchCriteria criteria)
    {
        Debug.WriteLine($"[MY] Searching for {criteria}");
        return Strategy.Search(fileStream, criteria);
    }

    public List<string> GetValues(Stream fileStream, string attributeName)
    {
        Debug.WriteLine($"[MY] Searching for {attributeName}");
        return Strategy.GetValues(fileStream, attributeName);
    }
}