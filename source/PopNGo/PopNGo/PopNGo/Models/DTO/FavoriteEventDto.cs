using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class FavoriteEventDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string EventID { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class FavoriteEventExtensions
    {
        public static PopNGo.Models.DTO.FavoriteEventDto ToDTO(this PopNGo.Models.FavoriteEvents FavoriteEvent)
        {
            return new PopNGo.Models.DTO.FavoriteEventDto
            {
                Id = FavoriteEvent.ID,
                UserId = FavoriteEvent.UserID,
                EventID = FavoriteEvent.EventID
            };
        }
    }
}