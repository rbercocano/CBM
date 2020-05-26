using Charcutarie.Models.Enums;
using Charcutarie.Models.ViewModels;
using FluentValidation;
using System;
using System.Data;

namespace Charcutarie.Api.Validators
{
    public class NewOrderItemValidator : AbstractValidator<NewOrderItem>
    {
        public NewOrderItemValidator()
        {
            RuleFor(x => x.OrderId).NotNull();
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.ProductId).NotNull();
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.OrderItemStatusId).IsInEnum();
            RuleFor(x => x.MeasureUnitId).IsInEnum();
        }
    }
    public class UpdateOrderItemValidator : AbstractValidator<UpdateOrderItem>
    {
        public UpdateOrderItemValidator()
        {
            RuleFor(x => x.OrderItemId).NotNull();
            RuleFor(x => x.OrderItemId).GreaterThan(0);
            RuleFor(x => x.OrderId).NotNull();
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.ProductId).NotNull();
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Quantity).NotNull();
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(x => x.OrderItemStatusId).IsInEnum();
            RuleFor(x => x.MeasureUnitId).IsInEnum();

        }
    }
}
