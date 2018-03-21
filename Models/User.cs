using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            Messages = new List<Message>();
            // Comments = new List<Comment>();
        }
        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password confirmation must match Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirmation { get; set; }

        public List<Message> Messages { get; set; }

        // public List<Comment> Comments { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}