using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise.Web.Validations
{
    public class TestValidation
    {
        public IEnumerable<string> TestRegisterRequestValidation(RegisterRequest request)
        {
            var validator = new RegisterRequestValidator();
            ValidationResult result = validator.Validate(request);
            if (!result.IsValid)
                return result.Errors.Select(r => r.ErrorMessage);

            return Enumerable.Empty<string>();
        }
    }

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            
            //for complete knowledge only
            //RuleSet("RuleSetName", () =>
            //{
            //    RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            //});

            //Inheritance
            //RuleFor(x => x.Name).SetInheritanceValidator(x=> {
            //    x.Add<DerivedClassName>(validationRulesName);
            //    x.Add<DerivedClassName>(validationRulesName);
            //});

            RuleFor(x => x.Name).NotEmpty().Length(0, 200);
            RuleFor(x => x.Email).NotEmpty().Length(0, 150).EmailAddress();
            RuleFor(x => x.Address).NotNull().SetValidator(new AddressValidator());
            RuleFor(x => x.Addresses).NotNull().SetValidator(new AddressesValidator());
        }
    }
    public class AddressValidator : AbstractValidator<AddressDto>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty().Length(0, 100);
            RuleFor(x => x.City).NotEmpty().Length(0, 40);
            RuleFor(x => x.State).NotEmpty().Length(0, 2);
            RuleFor(x => x.ZipCode).NotEmpty().Length(0, 5);
        }
    }

    public class AddressesValidator : AbstractValidator<AddressDto[]>
    {
        public AddressesValidator()
        {
            RuleFor(x => x)
                .Must(x => x?.Length >= 1 && x.Length <= 3)
                .WithMessage("The number of addresses must be between 1 and 3")
                .ForEach(x =>
                {
                    x.NotNull();
                    x.SetValidator(new AddressValidator());
                });
        }
    }

    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public AddressDto Address { get; set; }
        public AddressDto[] Addresses { get; set; }
    }
    public class AddressDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
