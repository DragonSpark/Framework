using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public interface IEntityDataBehaviorRepository
    {
        IEnumerable<IEntityDataBehavior> Retrieve( Type entityType );
    }
}