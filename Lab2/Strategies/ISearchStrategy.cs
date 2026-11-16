using Lab2.Models;

namespace Lab2.Strategies;

public interface ISearchStrategy
{
    string Name { get; }
    List<Scientist> Search(Stream xmlStream, ISearchCriteria criteria);
    List<string> GetValues(Stream xmlStream, string attributeName);
}