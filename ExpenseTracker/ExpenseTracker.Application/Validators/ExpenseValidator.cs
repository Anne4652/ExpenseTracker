using ExpenseTracker.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Validators
{
    public class ExpenseValidator : AbstractValidator<Expense>
    {
        public ExpenseValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("A valid category ID is required.");
        }
    }
}
