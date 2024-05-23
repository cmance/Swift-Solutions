using PopNGo.Models;

namespace PopNGo.Services
{
    public interface IOpenAiService
    {
        Task<String> FindMostRelevantWordFromString(string description);
    }
}
