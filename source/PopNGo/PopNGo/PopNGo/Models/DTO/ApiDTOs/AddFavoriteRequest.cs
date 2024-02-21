using PopNGo.Models.DTO;
using PopNGo.Models;

namespace PopNGo.Models.DTO
{
    public class AddFavoriteRequest
    {
        //This class combines the FavoriteEventDto and Event classes to allow for the addition of a favorite event
        //Since the FavoriteEventDto and Event classes are in different namespaces, this class is necessary to allow 
        //for the addition of a favorite event

        //MVC does not support two [FromBody] parameters in a single method, so this class is necessary to allow for the addition of a favorite event
        public FavoriteEventDto Favorite { get; set; }
        public PopNGo.Models.DTO.Event Event { get; set; }
    }
}