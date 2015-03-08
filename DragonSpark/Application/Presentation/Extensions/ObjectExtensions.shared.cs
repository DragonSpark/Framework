using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Presentation.Extensions
{
    public static class ObjectExtensions
    {
        public static bool Validate( this object target, bool allProperties = true )
        {
            var context = new ValidationContext( target, null, null );
            var validationResults = new List<ValidationResult>();
            var result = Validator.TryValidateObject( target, context, validationResults, allProperties );
            return result;
        }
    }
}