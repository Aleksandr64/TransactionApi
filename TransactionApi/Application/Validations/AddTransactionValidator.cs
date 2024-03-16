using FluentValidation;
using TransactionApi.Domain.DTOs;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Validations;

//Validation Transaction from CSV file.
public class AddTransactionValidator : AbstractValidator<TransactionCSVRequest>
{
    public AddTransactionValidator()
    {
        RuleFor(t => t.TransactionId).NotEmpty();
        
        RuleFor(transaction => transaction.Name).NotEmpty();
        
        RuleFor(transaction => transaction.Email).NotEmpty().EmailAddress();

        RuleFor(transaction => transaction.Amount).GreaterThan(0);

        RuleFor(transaction => transaction.TransactionDate).NotEmpty();

        RuleFor(transaction => transaction.ClientLocation).NotEmpty();
    }
}