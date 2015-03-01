using System.Collections.Generic;
using System.Collections.Specialized;

namespace DragonSpark.Application.Presentation.ComponentModel
{
	public interface IObservableCollection<T> : IList<T>, IViewObject, INotifyCollectionChanged
	{
		void AddRange( IEnumerable<T> items );

		void RemoveRange( IEnumerable<T> items );
	}
}