using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace PopularNetLibraries.FluentValidators
{
    public class BasicValidationTests
    {
        private static class Sample01
        {
            public class Employee
            {
                public string Firstname { get; init; }
                public string Lastname { get; init; }
            }

            public class EmployeeValidator : AbstractValidator<Employee>
            {
                public EmployeeValidator()
                {
                    RuleFor(employee => employee.Lastname).NotNull().MinimumLength(2);
                }
            }
        }
        
        [Fact]
        public void Employee_Valid()
        {
            var employee = new Sample01.Employee {Firstname = "Bogdan", Lastname = "Polak"};
            var validator = new Sample01.EmployeeValidator();
            
            var testValidationResult = validator.TestValidate(employee);

            testValidationResult.ShouldNotHaveAnyValidationErrors();
        }
        
        [Fact]
        public void Employee_Invalid_LastNameHas1Char()
        {
            var employee = new Sample01.Employee {Firstname = "Bogdan", Lastname = "P"};
            var validator = new Sample01.EmployeeValidator();
            
            var validationResult = validator.Validate(employee);
            Assert.False(validationResult.IsValid,"Lastname with 1 char - IsValid should be: false");
            Assert.Single(validationResult.Errors);
            Assert.Equal(
                "The length of 'Lastname' must be at least 2 characters. You entered 1 characters.",
                validationResult.Errors[0].ErrorMessage);
            Assert.Equal("Lastname", validationResult.Errors[0].PropertyName);
        }
    }
}