using Charcutarie.Models.ViewModels;
using FluentValidation;
using Charcutarie.Core.Extensions;
using System.Security.Cryptography;
using Charcutarie.Core.Security;

namespace Charcutarie.Api.Validators
{
    public class ClientRegistrationValidator : AbstractValidator<ClientRegistration>
    {
        public ClientRegistrationValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).Length(0, 100);
            RuleFor(x => x.Password).Must((c, p) =>
            {
                return c.PasswordConfirmation == p;
            }).WithMessage("As senhas não coincidem");

            RuleFor(x => x.DbaName).Must((c, p) =>
            {
                if (c.CustomerTypeId == 1) return true;
                return !string.IsNullOrWhiteSpace(p);
            }).WithMessage("Razão Social não pode ser nulo para clientes do tipo Pessoa Jurídica");

            RuleFor(x => x.SocialIdentifier).Must((c, p) =>
            {
                if (c.CustomerTypeId == 1) return true;
                return p.IsCnpj();
            }).WithMessage("CNPJ Inválido");

            RuleFor(x => x.SocialIdentifier).Must((c, p) =>
            {
                if (c.CustomerTypeId == 2) return true;
                return p.IsCpf();
            }).WithMessage("CPF Inválido");

            RuleFor(x => x.Email).Must((c, p) =>
            {
                return p.IsEmail();
            }).WithMessage("E-mail Inválido");
            RuleFor(x => x.Password).Must(p =>
            {
                return Password.IsPasswordSecure(p);
            }).WithMessage("A senha não atende aos requisitos mínimos de segurança. Tamanho mínimo de 8 caractéres,ao menos uma letra maíuscula, minúscula, número e caracteres especias.");
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
