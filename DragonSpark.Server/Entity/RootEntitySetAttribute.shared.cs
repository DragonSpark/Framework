using System;

namespace DragonSpark.Application.Communication.Entity
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RootEntitySetAttribute : EntitySetAttribute
    {
        public RootEntitySetAttribute()
        {
            IsRoot = true;
        }
    }
}