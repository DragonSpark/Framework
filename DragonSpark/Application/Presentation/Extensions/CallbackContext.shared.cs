using System;

namespace DragonSpark.Application.Presentation.Extensions
{
    struct CallbackContext<TItem> where TItem : class
    {
        readonly TItem item;
        readonly Delegate callback;

        public CallbackContext( TItem item, Delegate callback )
        {
            this.item = item;
            this.callback = callback;
        }

        public override bool Equals(object obj)
        {
            return obj is CallbackContext<TItem> && Equals( (CallbackContext<TItem>)obj );
        }

        bool Equals( CallbackContext<TItem> obj )
        {
            return Equals( obj.item, item ) && Equals( obj.callback, callback );
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ( ( item != null ? item.GetHashCode() : 0 ) * 397 ) ^ ( callback != null ? callback.GetHashCode() : 0 );
            }
        }
    }
}