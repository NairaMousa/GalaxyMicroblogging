using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Helper.CustomAttributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSizeBytes;

        public MaxFileSizeAttribute(long maxFileSizeBytes)
        {
            _maxFileSizeBytes = maxFileSizeBytes;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file != null && file.Length > _maxFileSizeBytes)
            {
                return new ValidationResult($"Maximum allowed file size is {_maxFileSizeBytes / 1024 / 1024} MB.");
            }

            return ValidationResult.Success;
        }
    }
}
