using System;
using DragonSpark.Application.Presentation.Converters;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public class EntityDataBehaviorConverter : ValueConverterBase<Type,object>
    {
        public static EntityDataBehaviorConverter Instance
        {
            get { return InstanceField; }
        }	static readonly EntityDataBehaviorConverter InstanceField = new EntityDataBehaviorConverter();

        [Dependency]
        public IEntityDataBehaviorRepository Repository { get; set; }

        protected override object PerformConversion( Type value, object parameter )
        {
            this.BuildUpOnce();
            var result = value.Transform( Repository.Retrieve );
            return result;
        }
    }
}