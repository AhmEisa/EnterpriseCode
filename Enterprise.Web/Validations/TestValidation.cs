using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

    public static class CustomValidators
    {
        public static IRuleBuilderOptionsConditions<T, IList<TElement>> ListMustContainNumberOfItems<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int? min = null, int? max = null)
        {
            return ruleBuilder.Custom((list, context) =>
            {

                if (min.HasValue && list.Count < min.Value) { context.AddFailure($"The list must contain {min.Value} items or more.It contains {list.Count} items"); }
                if (max.HasValue && list.Count > max.Value) { context.AddFailure($"The list must contain {min.Value} items or fewer.It contains {list.Count} items"); }
            });
        }
    }

    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {

            // to stop validation on failure for all validations on startup configureServices method
            //ValidatorOptions.Global.CascadeMode = CascadeMode.Stop;
            //CascadeMode = CascadeMode.Stop; // class level


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
            RuleFor(x => x.Addresses).ListMustContainNumberOfItems(1, 3);

            When(x => x.Email == null, () =>
            {
                RuleFor(x => x.Phone)
                .NotEmpty();
            });

            When(x => x.Phone == null, () =>
            {
                RuleFor(x => x.Email)
                .NotEmpty();
            });
            RuleFor(x => x.Email)
               .NotEmpty()
               .Length(0, 150)
               .EmailAddress()
               .When(x => x.Email != null);

            RuleFor(x => x.Phone)
                 .NotEmpty()
                 .Matches("^[2-9][0-9]{9}$")
                 .When(x => x.Phone == null)
                 .WithMessage("The phone is incorrect");


            When(x => x.Email != null, () =>
            {
                RuleFor(x => x.Email)
                .NotEmpty()
                .Length(0, 150)
                .EmailAddress()
                .When(x => x.Phone == null);

            }).Otherwise(() =>
            {
                RuleFor(x => x.Phone)
                  .NotEmpty()
                  .Matches("^[2-9][0-9]{9}$")
                  .When(x => x.Email == null)
                  .WithMessage("The phone is incorrect");

            });

            RuleFor(x => x.Phone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Matches("^[2-9][0-9]{9}$").WithMessage("The phone is incorrect")//or
                .Must(x => Regex.IsMatch(x, "")).When(x => x.Phone != null, ApplyConditionTo.CurrentValidator).WithMessage("The phone is incorrect");

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
                .WithMessage("The number of addresses must be between 1 and 3") // or use custom validation
                .ListMustContainNumberOfItems(1, 3)
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
        public string Phone { get; set; }
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
