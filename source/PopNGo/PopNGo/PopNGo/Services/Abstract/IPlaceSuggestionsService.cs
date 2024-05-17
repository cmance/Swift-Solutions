
using PopNGo.Models;

namespace PopNGo.Services
{
    public interface IPlaceSuggestionsService
    {
        public Task<IEnumerable<PlaceSuggestion>> SearchPlaceSuggestion(string query, string coordinates);

    }
}
