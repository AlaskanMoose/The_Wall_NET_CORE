using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class Message : BaseEntity
    {

        public Message()
        {
            Comments = new List<Comment>();
        }
        [Key]
        public int MessageId { get; set; }

        [Required]
        [Display(Name = "Message Content")]
        public string MessageContent { get; set; }

        public int user_id { get; set; }

        public User Poster { get; set; }

        public List<Comment> Comments { get; set; }

        public DateTime CreatedAt { get; set; }


        public bool Deletable { 
            get
            {
                var diff = DateTime.Now - CreatedAt;
                return (diff.TotalMinutes <= 10000);
            }
        }
    }
}