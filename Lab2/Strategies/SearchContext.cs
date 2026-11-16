using Lab2.Models;

namespace Lab2.Strategies;

public class SearchContext(ISearchStrategy searchStrategy)
{
    public ISearchStrategy Strategy { get; set; } = searchStrategy;
    
    public List<Scientist> Search(Stream fileStream, ISearchCriteria criteria) => Strategy.Search(fileStream, criteria);
    public List<string> GetValues(Stream fileStream, string attributeName) => Strategy.GetValues(fileStream, attributeName);
}