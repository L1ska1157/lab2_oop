using Lab2.Models;

namespace Lab2.Strategies;

public class SearchContext(ISearchStrategy searchStrategy)
{
    public ISearchStrategy Strategy { get; set; } = searchStrategy;
    
    public List<Scientist> Search(string filePath, ISearchCriteria criteria) => Strategy.Search(filePath, criteria);
    public List<string> GetValues(string filePath, string attributeName) => Strategy.GetValues(filePath, attributeName);
}