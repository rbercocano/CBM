using Charcutarie.Models.ViewModels;
using FluentValidation;

namespace Charcutarie.Api.Validators
{
    public class NewCorpClientValidator : AbstractValidator<ClientRegistration>
    {
        public NewCorpClientValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(0, 100);
            RuleFor(x => x.DBAName).NotEmpty();
            RuleFor(x => x.DBAName).Length(0, 100);
            RuleFor(x => x.Active).NotNull();
        }
    }
    public class UpdateCorpClientValidator : AbstractValidator<UpdateCorpClient>
    {
        public UpdateCorpClientValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(0, 100);
            RuleFor(x => x.DBAName).NotEmpty();
            RuleFor(x => x.DBAName).Length(0, 100);
            RuleFor(x => x.Active).NotNull();
            RuleFor(x => x.CorpClientId).NotNull();
            RuleFor(x => x.CorpClientId).GreaterThan(0);
        }
    }
}
