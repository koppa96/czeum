using System;
using System.ComponentModel.DataAnnotations;

namespace Czeum.DAL.Entities
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        public string Sender { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }

        public Match Match { get; set; }
    }
}