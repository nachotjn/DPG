using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


public class MaxWinningNumbersAttribute : ValidationAttribute
{
    private readonly int _maxCount;

    public MaxWinningNumbersAttribute(int maxCount)
    {
        _maxCount = maxCount;
        ErrorMessage = $"The winning numbers cannot exceed {_maxCount}.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var list = value as List<int>;

        if (list != null && list.Count > _maxCount)
        {
            return new ValidationResult(ErrorMessage);  
        }

        return ValidationResult.Success;
    }
}
