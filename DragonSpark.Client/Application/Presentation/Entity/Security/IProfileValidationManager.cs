using System;

namespace DragonSpark.Application.Presentation.Entity.Security
{
    public interface IProfileValidationManager
    {
        void Validate( Action<ProfileValidationConfirmation> callback );
    }
}