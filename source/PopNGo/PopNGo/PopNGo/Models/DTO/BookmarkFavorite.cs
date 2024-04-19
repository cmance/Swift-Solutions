using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class BookmarkFavorite
    {
        public int Id { get; set; }
        public string BookmarkListTitle { get; set; }
        public string ApiEventId { get; set; }
    }
}
