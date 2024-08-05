using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using System;
using System.ComponentModel.DataAnnotations;

namespace movieApp.web.Validators
{
    public class BirthDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime datetime = Convert.ToDateTime(value);

            return datetime<= DateTime.Now;
        }
    }
}
