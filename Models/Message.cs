using System;
using System.ComponentModel.DataAnnotations;

namespace DreamDay.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public string SenderId { get; set; }
        public string ReceiverId { get; set; }

        public int WeddingId { get; set; }

        public string Content { get; set; }

        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}
