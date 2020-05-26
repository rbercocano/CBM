using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using FluentValidation;
using System;
using System.Data;

namespace Charcutarie.Api.Validators
{
    public class NewOrderValidator : AbstractValidator<NewOrder>
    {
        public NewOrderValidator()
        {
            RuleFor(x => x.CustomerId).NotNull();
            RuleFor(x => x.CustomerId).GreaterThan(0);
            RuleFor(x => x.CompleteBy).GreaterThan(DateTime.Now);
            RuleFor(x => x.OrderItems).NotEmpty();

        }
    }
    public class UpdateOrderValidator : AbstractValidator<UpdateOrder>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.CompleteBy).NotNull();
            RuleFor(x => x.PaymentStatusId).IsInEnum();

        }
    }
}
