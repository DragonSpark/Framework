using System;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    [AttributeUsage( AttributeTargets.Class )]
    public class EntityDataBehaviorAttribute : Attribute, IItemProfile
    {
        readonly Type behaviorType;

        public EntityDataBehaviorAttribute( Type behaviorType )
        {
            this.behaviorType = behaviorType;
        }

        public Type BehaviorType
        {
            get { return behaviorType; }
        }

        public string Name { get; set; }

        Type IItemProfile.ItemType
        {
            get { return behaviorType; }
        }

        string IItemProfile.ItemName
        {
            get { return Name; }
        }
    }
}