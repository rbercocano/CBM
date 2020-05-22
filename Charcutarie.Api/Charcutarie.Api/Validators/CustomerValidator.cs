using Charcutarie.Models.ViewModels;
using FluentValidation;

namespace Charcutarie.Api.Validators
{
    public class NewPersonCustomerValidator : AbstractValidator<NewPersonCustomer>
    {
        public NewPersonCustomerValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(0, 100);
        }
    }
    public class NewCompanyCustomerValidator : AbstractValidator<NewCompanyCustomer>
    {
        public NewCompanyCustomerValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(0, 100);
            RuleFor(x => x.DBAName).NotEmpty();
            RuleFor(x => x.DBAName).Length(0, 100);
        }
    }
    public class UpdatePersonCustomerValidator : AbstractValidator<UpdatePersonCustomer>
    {
        public UpdatePersonCustomerValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(0, 100);
        }
    }
    public class UpdateCompanyCustomerValidator : AbstractValidator<UpdateCompanyCustomer>
    {
        public UpdateCompanyCustomerValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(0, 100);
            RuleFor(x => x.DBAName).NotEmpty();
            RuleFor(x => x.DBAName).Length(0, 100);
        }
    }
}
