using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MenuWebapi.Models.Data;
namespace MenuWebapi.Models.Annotations
{
    public class EmailUnique : ValidationAttribute
    {
        public EmailUnique()
        {
        }
        public override object TypeId => base.TypeId;
        public override bool RequiresValidationContext => base.RequiresValidationContext;
        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool IsDefaultAttribute()
        {
            return base.IsDefaultAttribute();
        }
        public override bool IsValid(object? value)
        {
            return base.IsValid(value);
        }
        public override bool Match(object? obj)
        {
            return base.Match(obj);
        }
        public override string? ToString()
        {
            return base.ToString();
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            var textValue = value.ToString();
            ApplicationDbContext? dbContext = validationContext.GetService<ApplicationDbContext>();
            if (dbContext != null && dbContext.Users != null)
            {
                if (dbContext.Users.Any(w => w.Email == textValue))
                {
                    return new ValidationResult("Email busy");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return new ValidationResult("Db context fail");
            }
        }
    }
}
