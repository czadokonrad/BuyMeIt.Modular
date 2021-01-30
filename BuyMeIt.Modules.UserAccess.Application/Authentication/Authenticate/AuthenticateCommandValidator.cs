using System.Linq;
using FluentValidation;

namespace BuyMeIt.Modules.UserAccess.Application.Authentication.Authenticate
{
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateCommandValidator()
        {
            this.RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("Login cannot be empty");

            this.RuleFor(x => x.Login)
                .Must(x => !x.Any(char.IsWhiteSpace))
                .WithMessage("Login cannot contain whitespaces");
                
            this.RuleFor(x => x.Login)
                .MinimumLength(5)
                .WithMessage("Login must be at least 5 characters long");
            
            this.RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password cannot be empty");
            
            this.RuleFor(x => x.Password)
                .MinimumLength(8)
                .WithMessage("Password has to be at least 8 characters long");
        }
    }
}