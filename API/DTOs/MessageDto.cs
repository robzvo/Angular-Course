using System;
using System.Text.Json.Serialization;

namespace API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderID { get; set; }
        public int RecipientID { get; set; }
        public string SenderUsername { get; set; }
        public string SenderPhotoUrl { get; set; }
        public string RecipientUsername { get; set; }
        public string RecipientPhotoUrl { get; set; }
        public string Content { get; set; }
        
        public DateTime? DateRead{get; set; }
        public DateTime DateSent{get; set; }
        [JsonIgnore]
        public bool SenderDeleted {get;set;}
        [JsonIgnore]
        public bool RecipientDeleted {get;set;}
    }
}