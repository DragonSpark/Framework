using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    [Singleton( typeof(IEntityDataBehaviorRepository), Priority = Priority.Lowest )]
    public class MetadataBasedEntityDataBehaviorRepository : IEntityDataBehaviorRepository
    {
        readonly IDictionary<Type,IEnumerable<IEntityDataBehavior>> cache = new Dictionary<Type, IEnumerable<IEntityDataBehavior>>();

        public IEnumerable<IEntityDataBehavior> Retrieve( Type entityType )
        {
            var result = cache.Ensure( entityType, x => x.GetAttributes<EntityDataBehaviorAttribute>().Cast<IItemProfile>().Select( y => y.Activated<IEntityDataBehavior>() ).ToArray() );
            return result;
        }
    }
}