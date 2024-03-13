using FluentValidation;
using TransactionApi.Domain.Model;

namespace TransactionApi.Application.Validations;

public class AddTransactionValidator : AbstractValidator<Transaction>
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