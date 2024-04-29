using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class UpdateBookmarkListName
    {
        public string OldBookmarkListTitle { get; set; }
        public string NewBookmarkListTitle { get; set; }
    }
}
