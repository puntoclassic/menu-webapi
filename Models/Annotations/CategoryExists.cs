using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MenuBackend.Models.Data;
namespace MenuBackend.Models.Annotations
{
    public class CategoryExists : ValidationAttribute
    {
        public CategoryExists()
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
            var intValue = Convert.ToInt32(value);
            ApplicationDbContext? dbContext = validationContext.GetService<ApplicationDbContext>();
            if (dbContext != null && dbContext.Categories != null)
            {
                if (dbContext.Categories.Any(w => w.Id == intValue))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Category not founded");
                }
            }
            else
            {
                return new ValidationResult("Db context fail");
            }
        }
    }
}
