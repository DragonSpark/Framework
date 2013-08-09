using System.ComponentModel;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public class SelectionChangingEventArgs<TItem> : CancelEventArgs
    {
        readonly TItem oldValue;
        readonly TItem newValue;

        public SelectionChangingEventArgs( TItem oldValue, TItem  newValue )
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }

        public TItem OldValue
        {
            get { return oldValue; }
        }

        public TItem NewValue
        {
            get { return newValue; }
        }
    }
}