﻿using System.ComponentModel.DataAnnotations;

namespace movieApp.web.Validators
{
    public class EmailProvidersAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = "";
            if (value != null)
            {
                email = value.ToString();
            }
            if(email.EndsWith("@gmail.com") || email.EndsWith("@hotmail.com"))
            {
                return ValidationResult.Success;

            }
            return new ValidationResult("hatalı eposta sunucusu");

        }
    }
}
