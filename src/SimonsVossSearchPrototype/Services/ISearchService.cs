using System.Threading.Tasks;

namespace SimonsVossSearchPrototype.Services
{
    public interface ISearchService
    {
        Task<SearchResponse> Search(string term); 
    }
}
