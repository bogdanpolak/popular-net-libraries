using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace PopularNetLibraries.FluentValidators
{
    public class BasicValidationTests
    {
        private class Employee
        {
            public string Firstname { get; init; }
            public string Lastname { get; init; }
        }

        private class EmployeeValidator : AbstractValidator<Employee>
        {
            public EmployeeValidator()
            {
                RuleFor(employee => employee.Lastname).NotNull().MinimumLength(2);
            }
        }
        
        [Fact]
        public void Employee_Valid()
        {
            var employee = new Employee {Firstname = "Bogdan", Lastname = "Polak"};
            var validator = new EmployeeValidator();
            
            var testValidationResult = validator.TestValidate(employee);

            testValidationResult.ShouldNotHaveAnyValidationErrors();
        }
        
        [Fact]
        public void Employee_Invalid_LastNameHas1Char()
        {
            var employee = new Employee {Firstname = "Bogdan", Lastname = "P"};
            var validator = new EmployeeValidator();
            
            var validationResult = validator.Validate(employee);
            Assert.False(validationResult.IsValid,"Lastname with 1 char - IsValid should be: false");
            Assert.Single(validationResult.Errors);
            Assert.Equal(
                "The length of 'Lastname' must be at least 2 characters. You entered 1 characters.",
                validationResult.Errors[0].ErrorMessage);
            Assert.Equal("Lastname", validationResult.Errors[0].PropertyName);
        }
        
        // TODO: Add tests for EmployeeValidator: Employee.FirstName is not empty (test null and empty string)
        // TODO: Add tests for EmployeeValidator: Invalid when Employee is empty
        // TODO: Add tests for EmployeeValidator: new properties BirthMonth / BirthDay (valid and invalid)
        // TODO: Add tests for EmployeeValidator: add Employee.Status - equal to one of 3 string values (see link below)
        // **** https://stackoverflow.com/questions/33959323/checking-if-parameter-is-one-of-3-values-with-fluent-validation
    }
}