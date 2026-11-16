using Lab2.Models;

namespace Lab2.Strategies;

public interface ISearchStrategy
{
    string Name { get; }
    
    List<Scientist> Search(string filePath, ISearchCriteria  searchCriteria);
    List<string> GetValues(string filePath, string attributeName);
}