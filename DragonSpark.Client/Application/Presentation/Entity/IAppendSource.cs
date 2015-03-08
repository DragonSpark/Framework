using System.Collections.Generic;

namespace DragonSpark.Application.Presentation.Entity
{
    interface IAppendSource
    {
        void Append( IEnumerable<object> items );
    }
}