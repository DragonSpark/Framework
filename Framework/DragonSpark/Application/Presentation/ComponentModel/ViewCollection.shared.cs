using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public partial class ViewCollection<T> : ObservableCollection<T>, IObservableCollection<T>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewCollection{T}"/> class.
		/// </summary>
		public ViewCollection()
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewCollection{T}"/> class.
		/// </summary>
		/// <param name="collection">The collection from which the elements are copied.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// The <paramref name="collection"/> parameter cannot be null.
		/// </exception>
        public ViewCollection( IEnumerable<T> collection ) : base( collection )
		{}

        /// <summary>
		/// Enables/Disables property change notification.
		/// </summary>
		public bool IsNotifying
        {
            get { return isNotifying; }
            set { isNotifying = value; }
        }   bool isNotifying = true;

        /// <summary>
		/// Notifies subscribers of the property change.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public void NotifyOfPropertyChange( string propertyName )
		{
			if ( IsNotifying )
				Threading.Application.Execute( () => RaisePropertyChangedEventImmediately( new PropertyChangedEventArgs( propertyName ) ) );
		}

		/// <summary>
		/// Raises a change notification indicating that all bindings should be refreshed.
		/// </summary>
		public void RefreshAllNotifications()
		{
			Threading.Application.Execute( () =>
			{
				OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
				OnPropertyChanged( new PropertyChangedEventArgs( string.Empty ) );
			} );
		}

		/// <summary>
		/// Inserts the item to the specified position.
		/// </summary>
		/// <param name="index">The index to insert at.</param>
		/// <param name="item">The item to be inserted.</param>
		protected override sealed void InsertItem( int index, T item )
		{
			Threading.Application.Execute( () => InsertItemBase( index, item ) );
		}

		/// <summary>
		/// Exposes the base implementation of the <see cref="InsertItem"/> function.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		/// <remarks>Used to avoid compiler warning regarding unverifiable code.</remarks>
		protected virtual void InsertItemBase( int index, T item )
		{
			base.InsertItem( index, item );
		}

		
		/// <summary>
		/// Sets the item at the specified position.
		/// </summary>
		/// <param name="index">The index to set the item at.</param>
		/// <param name="item">The item to set.</param>
		protected sealed override void SetItem(int index, T item) {
			Threading.Application.Execute(() => SetItemBase(index, item));
		}

		/// <summary>
		/// Exposes the base implementation of the <see cref="SetItem"/> function.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		/// <remarks>Used to avoid compiler warning regarding unverifiable code.</remarks>
		protected virtual void SetItemBase(int index, T item) {
			base.SetItem(index, item);
		}

		/// <summary>
		/// Removes the item at the specified position.
		/// </summary>
		/// <param name="index">The position used to identify the item to remove.</param>
		protected sealed override void RemoveItem(int index) {
			Threading.Application.Execute(() => RemoveItemBase(index));
		}

		/// <summary>
		/// Exposes the base implementation of the <see cref="RemoveItem"/> function.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <remarks>Used to avoid compiler warning regarding unverifiable code.</remarks>
		protected virtual void RemoveItemBase(int index) {
			base.RemoveItem(index);
		}

		/// <summary>
		/// Clears the items contained by the collection.
		/// </summary>
		protected sealed override void ClearItems() {
			Threading.Application.Execute(ClearItemsBase);
		}

		/// <summary>
		/// Exposes the base implementation of the <see cref="ClearItems"/> function.
		/// </summary>
		/// <remarks>Used to avoid compiler warning regarding unverifiable code.</remarks>
		protected virtual void ClearItemsBase() {
			base.ClearItems();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Collections.ObjectModel.ObservableCollection`1.CollectionChanged"/> event with the provided arguments.
		/// </summary>
		/// <param name="e">Arguments of the event being raised.</param>
		protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e) {
			if(IsNotifying)
				base.OnCollectionChanged(e);
		}

		/// <summary>
		/// Raises the PropertyChanged event with the provided arguments.
		/// </summary>
		/// <param name="e">The event data to report in the event.</param>
		protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
			if(IsNotifying)
				base.OnPropertyChanged(e);
		}

		void RaisePropertyChangedEventImmediately(PropertyChangedEventArgs e) {
			OnPropertyChanged(e);
		}

		/// <summary>
		/// Adds the range.
		/// </summary>
		/// <param name="items">The items.</param>
		public virtual void AddRange(IEnumerable<T> items) {
			Threading.Application.Execute(() => {
			    var previousNotificationSetting = IsNotifying;
			    IsNotifying = false;
			    var index = Count;
			    foreach(var item in items) {
			        InsertItemBase(index, item);
			        index++;
			    }
			    IsNotifying = previousNotificationSetting;
			    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			    OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
			});
		}

		/// <summary>
		/// Removes the range.
		/// </summary>
		/// <param name="items">The items.</param>
		public virtual void RemoveRange(IEnumerable<T> items) {
			Threading.Application.Execute(() => {
			                         	var previousNotificationSetting = IsNotifying;
			                         	IsNotifying = false;
			                         	foreach(var item in items) {
			                         		var index = IndexOf(item);
			                         		RemoveItemBase(index);
			                         	}
			                         	IsNotifying = previousNotificationSetting;
			                         	OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			                         	OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
			});
		}
	}
}