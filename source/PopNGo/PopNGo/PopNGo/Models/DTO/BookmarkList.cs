using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class BookmarkList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public int FavoriteEventQuantity { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class BookmarkListExtensions
    {
        public static PopNGo.Models.DTO.BookmarkList ToDTO(this PopNGo.Models.BookmarkList BookmarkList)
        {
            return new PopNGo.Models.DTO.BookmarkList
            {
                Id = BookmarkList.Id,
                Title = BookmarkList.Title,
                UserId = BookmarkList.UserId,
                FavoriteEventQuantity = BookmarkList.FavoriteEvents.Count
            };
        }
    }
}
