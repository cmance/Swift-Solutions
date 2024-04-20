using PopNGo.Models;

namespace PopNGo.Services
{
    public interface IDistanceCalculatorService
    {
        Task<DistanceReturn> CalculateDistance(string location, List<string> events, string units);
    }
}