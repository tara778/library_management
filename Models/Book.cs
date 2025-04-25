using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace library_management.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(13, ErrorMessage = "ISBN must be 10 or 13 characters", MinimumLength = 10)]
        [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "Invalid ISBN format")]
        public string? ISBN { get; set; }

        [Required]
        [Display(Name = "Publication Year")]
        [Range(1000, 2100, ErrorMessage = "Invalid year")]
        public int PublicationYear { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        public int QuantityAvailable { get; set; }

        // ... other properties ...
        [Required(ErrorMessage = "The Author field is required")]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        [ValidateNever]  // Prevent validation on navigation property
        public virtual Author Author { get; set; } = null!;

        // Navigation property for Loans
        public virtual ICollection<Loan> Loans { get; set; } = new HashSet<Loan>();
    }
}