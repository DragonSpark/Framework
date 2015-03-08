using System.Security.Principal;

namespace DragonSpark.Application.Presentation.Entity.Security
{
    public interface IProfileValidator
    {
        bool IsValid( IPrincipal principal );
    }
}