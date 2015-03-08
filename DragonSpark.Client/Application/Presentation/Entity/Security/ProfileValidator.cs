using System.Security.Principal;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Entity.Security
{
    [Singleton( typeof(IProfileValidator), Priority = Priority.Lowest )]
    public class ProfileValidator : IProfileValidator
    {
        public bool IsValid( IPrincipal principal )
        {
            var result = !principal.Identity.IsAuthenticated || principal.Identity.Validate();
            return result;
        }
    }
}