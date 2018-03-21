using System;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class Comment : BaseEntity
    {
        [Key]
        public int CommentId { get; set; }

        [Required]
        [Display(Name = "Comment Content")]
        public string CommentContent { get; set; }

        [Required]
        public int user_id { get; set; }

        public User Commenter { get; set; }

        [Required]
        public int message_id { get; set; }

        public Message Message { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}