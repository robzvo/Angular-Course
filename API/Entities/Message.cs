using System;

namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderID { get; set; }
        public int RecipientID { get; set; }
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public bool SenderDeleted{get;set;}
        public bool RecipientDeleted{get;set;}
        public DateTime? DateRead{get; set; }
        public DateTime DateSent{get; set; } = DateTime.UtcNow;
        public AppUser Sender { get; set; }
        public AppUser Recipient { get; set; }
    }
}