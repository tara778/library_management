using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace library_management.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; } // Nullable to handle optional dates

        [StringLength(500, ErrorMessage = "Biography cannot exceed 500 characters")]
        public string? Biography { get; set; }

        // Navigation property for related books
        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}