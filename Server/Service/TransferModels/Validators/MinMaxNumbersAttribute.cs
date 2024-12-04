using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


public class MinMaxNumbersAttribute : ValidationAttribute{
    private readonly int _minCount;
    private readonly int _maxCount;

    public MinMaxNumbersAttribute(int minCount, int maxCount){
        _minCount = minCount;
        _maxCount = maxCount;
        ErrorMessage = $"The number sequence must contain between {_minCount} and {_maxCount} numbers.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext){
        var list = value as List<int>;

        if (list != null){
            if (list.Count < _minCount || list.Count > _maxCount){
                return new ValidationResult(ErrorMessage);
            }
        }

        return ValidationResult.Success;
    }
}
