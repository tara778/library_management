using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace library_management.Models
{
    public class Loan
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Book is required")]
        [Display(Name = "Book")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Member is required")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Loan date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Loan Date")]
        public DateTime LoanDate { get; set; } = DateTime.UtcNow; // Ensure UTC

        [Required(ErrorMessage = "Due date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14); // Ensure UTC

        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "Fine Amount")]
        [Range(0, 1000, ErrorMessage = "Fine must be between 0 and 1000")]
        public decimal? FineAmount { get; set; }

        // Navigation properties
        [ForeignKey("BookId")]
        [ValidateNever]
        public virtual Book Book { get; set; } = null!;

        [ForeignKey("MemberId")]
        [ValidateNever]
        public virtual Member Member { get; set; } = null!;
    }
}
