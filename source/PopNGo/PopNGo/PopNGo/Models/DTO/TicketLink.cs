using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace PopNGo.Models.DTO
{
    public class TicketLink
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Source { get; set; } = "";
        public string Link { get; set; } = "";
        public virtual Event Event { get; set; }
    }
}

namespace PopNGo.ExtensionMethods
{
    public static class TicketLinkExtensions
    {
        public static PopNGo.Models.DTO.TicketLink ToDTO(this PopNGo.Models.TicketLink TicketLink)
        {
            return new PopNGo.Models.DTO.TicketLink
            {
                Source = TicketLink.Source,
                Link = TicketLink.Link,
            };
        }
    }
}
