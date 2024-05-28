using PopNGo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PopNGo.Services
{
    public interface IMapDirectionsService
    {
        public Task<IEnumerable<DirectionDetail>> GetDirections(string startAddress, string endAddress);
    }
}
