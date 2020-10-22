using Charcutarie.Models.ViewModels;
using FluentValidation;
using Charcutarie.Core.Extensions;
using System.Security.Cryptography;
using Charcutarie.Core.Security;

namespace Charcutarie.Api.Validators
{
    public class NewTransactionValidator : AbstractValidator<NewTransaction>
    {
        public NewTransactionValidator()
        {
            RuleFor(x => x.MerchantName).NotEmpty().WithMessage("Mercante não pode ser nulo");
            RuleFor(x => x.MerchantName).MaximumLength(50).WithMessage("Mercante não pode possuir mais que 50 caractéres");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Descrição não pode ser nulo");
            RuleFor(x => x.Description).MaximumLength(200).WithMessage("Descrição não pode possuir mais que 200 caractéres");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Valor deve ser maior que ZERO");
        }
    }
}
