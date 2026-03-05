using System;
using System.Text.RegularExpressions;
using EmployeeRegistry.Application.Commands;
using EmployeeRegistry.Application.Interfaces;
using FluentValidation;

namespace EmployeeRegistry.Application.Validators
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _repository;

        public CreateEmployeeCommandValidator(IEmployeeRepository repository)
        {
            _repository = repository;

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.NID)
                .NotEmpty()
                .Must(BeValidNID).WithMessage("NID must be 10 or 17 digits.")
                .Must(BeUniqueNID).WithMessage("NID already exists.");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .Must(BeValidBangladeshPhone)
                .WithMessage("Phone must be a valid Bangladeshi number (+8801 or 01 format).");

            RuleFor(x => x.Department).NotEmpty();

            RuleFor(x => x.BasicSalary)
                .GreaterThan(0);

            When(x => x.Spouse != null, () =>
            {
                RuleFor(x => x.Spouse.Name).NotEmpty();

                RuleFor(x => x.Spouse.NID)
                    .NotEmpty()
                    .Must(BeValidNID)
                    .WithMessage("Spouse NID must be 10 or 17 digits.");
            });

            RuleForEach(x => x.Children).ChildRules(child =>
            {
                child.RuleFor(c => c.Name).NotEmpty();
                child.RuleFor(c => c.DateOfBirth).LessThan(DateTime.Today);
            });
        }

        private bool BeValidNID(string nid)
        {
            if (string.IsNullOrEmpty(nid)) return false;

            return Regex.IsMatch(nid, @"^\d{10}$|^\d{17}$");
        }

        private bool BeValidBangladeshPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;

            return Regex.IsMatch(phone, @"^(?:\+8801|01)[3-9]\d{8}$");
        }

        private bool BeUniqueNID(string nid)
        {
            return !_repository.IsNidExistsAsync(nid)
                .GetAwaiter()
                .GetResult();
        }
    }
}
