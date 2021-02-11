
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Test.DTO.Requests;

namespace Test.ValidationRules
{
    public class RegisterValidations:AbstractValidator<RegisterRequest>
    {
        public RegisterValidations()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid Email format").NotEmpty().WithMessage("Can't be empty");
            RuleFor(x => x.Password2).Equal(x => x.Password).WithMessage("Passwords don't match").NotEmpty().WithMessage("Can't be empty").MinimumLength(6).WithMessage("Minimum length should be 6");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password can't be empty").MinimumLength(6).WithMessage("Minimum length should be 6");
            RuleFor(x => x.FirstName).NotEmpty().Matches("^[a-zA-Z]+$").WithMessage("Should contain only letters").MinimumLength(2).WithMessage("Minimum length should be 2");
            RuleFor(x => x.LastName).NotEmpty().Matches("^[a-zA-Z]+$").WithMessage("Should contain only letters").MinimumLength(2).WithMessage("Minimum length should be 2");
        }
    }
}
