using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [MaxLength(250, ErrorMessage = "Description must not exceed 250 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "A valid Category ID is required.")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
