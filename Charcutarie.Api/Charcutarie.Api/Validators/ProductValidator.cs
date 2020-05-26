using Charcutarie.Models.ViewModels;
using FluentValidation;

namespace Charcutarie.Api.Validators
{
    //public class NewProductValidator : AbstractValidator<NewProduct>
    //{
    //    public NewProductValidator()
    //    {
    //        RuleFor(x => x.Name).NotEmpty();
    //        RuleFor(x => x.Name).Length(0, 100);
    //        RuleFor(x => x.MeasureUnitId).IsInEnum();
    //        RuleFor(x => x.Price).NotNull();
    //        RuleFor(x => x.ActiveForSale).NotNull();
    //    }
    //}
    public class UpdateProductValidator : AbstractValidator<UpdateProduct>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(0, 100);
            RuleFor(x => x.MeasureUnitId).IsInEnum();
            RuleFor(x => x.Price).NotNull();
            RuleFor(x => x.ActiveForSale).NotNull();
            RuleFor(x => x.ProductId).NotNull();
            RuleFor(x => x.ProductId).GreaterThan(0);
        }
    }
}
