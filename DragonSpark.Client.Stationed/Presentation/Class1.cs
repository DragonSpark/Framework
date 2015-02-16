using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using DragonSpark.Activation;
using DragonSpark.Application.Client.Commanding;
using DragonSpark.Application.Client.Extensions;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism;
using Xceed.Wpf.Toolkit;

namespace DragonSpark.Application.Client.Presentation
{
	public class ViewCollection<T> : ObservableCollection<T>, IObservableCollection<T>
	{
		readonly Collection<T> adding = new Collection<T>();
		readonly Collection<T> removing = new Collection<T>();

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

		public new IEnumerator<T> GetEnumerator()
		{
			var result = adding.ToArray().Concat( Items.ToArray() ).Except( removing.ToArray() ).ToList().GetEnumerator();
			return result;
		}

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
			adding.Add( item );
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
			adding.Remove( item );
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
		protected sealed override void RemoveItem(int index)
		{
			var item = Items.ElementAtOrDefault( index );
			item.NotNull( removing.Add );
			Threading.Application.Execute(() => RemoveItemBase(index));
		}

		/// <summary>
		/// Exposes the base implementation of the <see cref="RemoveItem"/> function.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <remarks>Used to avoid compiler warning regarding unverifiable code.</remarks>
		protected virtual void RemoveItemBase(int index) {
			var item = Items.ElementAtOrDefault( index );
			item.NotNull( x => removing.Remove( x ) );
			base.RemoveItem(index);
		}

		/// <summary>
		/// Clears the items contained by the collection.
		/// </summary>
		protected sealed override void ClearItems() {
			removing.AddRange( Items );
			Threading.Application.Execute(ClearItemsBase);
		}

		/// <summary>
		/// Exposes the base implementation of the <see cref="ClearItems"/> function.
		/// </summary>
		/// <remarks>Used to avoid compiler warning regarding unverifiable code.</remarks>
		protected virtual void ClearItemsBase() {
			removing.Clear();
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
		public virtual void AddRange(IEnumerable<T> items)
		{
			var range = items.ToArray();
			adding.AddRange( range );
			Threading.Application.Execute(() => {
				var previousNotificationSetting = IsNotifying;
				IsNotifying = false;
				var index = Count;
				foreach(var item in range) {
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
			var range = items.ToArray();
			removing.AddRange( range );
			Threading.Application.Execute(() => {
										var previousNotificationSetting = IsNotifying;
										IsNotifying = false;
										foreach(var item in range) {
											var index = IndexOf(item);
											RemoveItemBase(index);
										}
										IsNotifying = previousNotificationSetting;
										OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
										OnPropertyChanged(new PropertyChangedEventArgs(string.Empty));
			});
		}

		/// <summary>
		/// Moves the item within the collection.
		/// </summary>
		/// <param name="oldIndex">The old position of the item.</param>
		/// <param name="newIndex">The new position of the item.</param>
		protected override sealed void MoveItem( int oldIndex, int newIndex )
		{
			Threading.Application.Execute( () => MoveItemBase( oldIndex, newIndex ) );
		}

		/// <summary>
		/// Exposes the base implementation fo the <see cref="MoveItem"/> function.
		/// </summary>
		/// <param name="oldIndex">The old index.</param>
		/// <param name="newIndex">The new index.</param>
		/// <remarks>Used to avoid compiler warning regarding unverificable code.</remarks>
		protected virtual void MoveItemBase( int oldIndex, int newIndex )
		{
			base.MoveItem( oldIndex, newIndex );
		}
	}

	public interface IAttachedObject : System.Windows.Interactivity.IAttachedObject
	{
		event EventHandler Attached;
		event EventHandler Detached;
	}

	public interface IObservableCollection<T> : IList<T>, IViewObject, INotifyCollectionChanged
	{
		void AddRange( IEnumerable<T> items );

		void RemoveRange( IEnumerable<T> items );
	}

	public interface IViewObject : INotifyPropertyChanged
	{
		bool IsNotifying { get; }

		void NotifyOfPropertyChange( string propertyName );

		void RefreshAllNotifications();
	}

	public static class PropertySupport
	{
		public static bool SetProperty<TItem>( ref TItem current, TItem assignment, System.Linq.Expressions.Expression expression, Action<string> notify )
		{
			var result = SetProperty( ref current, assignment, expression.GetMemberInfo().Name, notify );
			return result;
		}

		public static bool SetProperty<TItem>( ref TItem current, TItem assignment, string name, Action<string> notify )
		{
			var result = !Equals( current, assignment );
			if ( result )
			{
				current = assignment;
				notify( name );
				// NotifyOfPropertyChange( expression );
			}
			return result;
		}
	}

	public class ViewAwareObject : ViewObject, IAttachedObject
	{
		readonly ViewAwareSupporter viewAwareSupporter;

		protected ViewAwareObject()
		{
			viewAwareSupporter = new ViewAwareSupporter( this );
		}

		#region IAttachedObject
		event EventHandler IAttachedObject.Attached
		{
			add { viewAwareSupporter.Attached += value; }
			remove { viewAwareSupporter.Attached -= value; }
		}

		event EventHandler IAttachedObject.Detached
		{
			add { viewAwareSupporter.Detached += value; }
			remove { viewAwareSupporter.Detached -= value; }
		}

		void System.Windows.Interactivity.IAttachedObject.Attach( DependencyObject dependencyObject )
		{
			viewAwareSupporter.Attach( dependencyObject );
			OnAttached();
		}

		protected virtual void OnAttached()
		{
			this.GetAllPropertyValuesOf<IAttachedObject>().Apply( x => x.Attach( AssociatedObject ) );
		}

		void System.Windows.Interactivity.IAttachedObject.Detach()
		{
			OnDetached();
		}

		protected virtual void OnDetached()
		{}

		public DependencyObject AssociatedObject
		{
			get { return viewAwareSupporter.AssociatedObject; }
		}
		#endregion
	}

	public class ViewAwareSupporter : IAttachedObject
	{
		readonly IAttachedObject owner;
		public event EventHandler Attached = delegate {}, Detached = delegate {};

		public ViewAwareSupporter( IAttachedObject owner )
		{
			this.owner = owner;
		}

		IEnumerable<IAttachedObject> DetermineItems()
		{
			var result = owner.GetAllPropertyValuesOf<IAttachedObject>()
				/*.Concat( owner.GetAllPropertyValuesOf<IEnumerable>().NotNull().SelectMany( x => x.OfType<IAttachedObject>() ) )
				.Concat( owner.GetAllPropertyValuesOf<IDictionary>().NotNull().SelectMany( x => x.Keys.OfType<IAttachedObject>() ) )
				.Where( x => x != owner )*/
				.NotNull().Distinct().ToArray();
			return result;
		}

		public void Attach( DependencyObject dependencyObject )
		{
			AssociatedObject = dependencyObject;
			Attached( owner, EventArgs.Empty );

			var items = DetermineItems();

			items.Where( x => x.AssociatedObject == null ).Apply( x => x.Attach( dependencyObject ) );
		}

		public void Detach()
		{
			Detached( owner, EventArgs.Empty );
			var items = DetermineItems();
			items.Apply( x => x.Detach() );
		}

		public DependencyObject AssociatedObject { get; private set; }
	}

	public class ViewObject : IViewObject
	{
		/// <summary>
		/// Creates an instance of <see cref="ViewObject"/>.
		/// </summary>
		public ViewObject()
		{
			IsNotifying = true;
		}

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		/// <summary>
		/// Enables/Disables property change notification.
		/// </summary>
		public bool IsNotifying { get; set; }

		/// <summary>
		/// Raises a change notification indicating that all bindings should be refreshed.
		/// </summary>
		public void RefreshAllNotifications()
		{
			NotifyOfPropertyChange( string.Empty );

			UpdateCommands();
		}

		protected virtual void UpdateCommands()
		{
			this.GetAllPropertyValuesOf<System.Windows.Input.ICommand>().Apply( x => x.Update() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Used as convenience." ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract expression name." )]
		protected bool SetProperty<TItem>( ref TItem current, TItem assignment, Expression<Func<TItem>> expression )
		{
			var result = PropertySupport.SetProperty( ref current, assignment, expression, NotifyOfPropertyChange );
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Used as convenience." ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract expression name." )]
		protected bool SetProperty<TItem>( ref TItem current, TItem assignment, [CallerMemberName] string propertyName = null )
		{
			var result = PropertySupport.SetProperty( ref current, assignment, propertyName, NotifyOfPropertyChange );
			return result;
		}

		/// <summary>
		/// Notifies subscribers of the property change.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public virtual void NotifyOfPropertyChange( string propertyName )
		{
			if ( IsNotifying )
			{
				Threading.Application.Execute( () => RaisePropertyChangedEventCore( propertyName ) );
			}
		}

		/// <summary>
		/// Notifies subscribers of the property change.
		/// </summary>
		/// <typeparam name="TProperty">The type of the property.</typeparam>
		/// <param name="expression">The property expression.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract expression." )]
		public virtual void NotifyOfPropertyChange<TProperty>( Expression<Func<TProperty>> expression )
		{
			NotifyOfPropertyChange( expression.GetMemberInfo().Name );
		}

		/// <summary>
		/// Raises the property changed event immediately.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		protected virtual void RaisePropertyChangedEventImmediately( string propertyName )
		{
			if ( IsNotifying )
			{
				RaisePropertyChangedEventCore( propertyName );
			}
		}

		void RaisePropertyChangedEventCore( string propertyName )
		{
			PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}

	public interface IDelegateWorker
	{
		IDelegateContext Execute( Action target );

		IDelegateContext Start( Action target );

		IDelegateContext Delay( Action target, TimeSpan time );
	}

	public class TaskDelegateWorker : IDelegateWorker
	{
		public IDelegateContext Execute( Action target )
		{
			var result = Start( target );
			return result;
		}

		public IDelegateContext Start( Action target )
		{
			var result = ResolveContext( target );
			return result;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Token source gets disposed when the Task does." )]
		static TaskContext ResolveContext( Action target )
		{
			var source = new CancellationTokenSource();
			var task = Task.Factory.StartNew( target, source.Token ).ContinueWith( x => x.Exception.NotNull( y => { throw y.InnerException; } ), source.Token );
			var result = new TaskContext( task, source );
			return result;
		}

		public IDelegateContext Delay( Action target, TimeSpan time )
		{
			var result = ResolveContext( target );
			result.Task.ContinueWith( async x => await Task.Delay( time ) );
			return result;
		}
	}

	public class TaskContext : IDelegateContext
	{
		readonly Task task;
		readonly CancellationTokenSource source;

		internal TaskContext( Task task, CancellationTokenSource source )
		{
			this.task = task;
			this.source = source;
		}

		public Task Task
		{
			get { return task; }
		}

		public CancellationTokenSource Source
		{
			get { return source; }
		}

		public object State
		{
			get { return task; }
		}

		public bool Cancel()
		{
			source.Cancel();
			return true;
		}
	}

	
	public interface IDelegateContext
	{
		object State { get; }

		bool Cancel();
	}

	public static class Threading
	{
		public static IDelegateWorker Application
		{
			get { return ServiceLocation.With<IDelegateWorkerProvider, IDelegateWorker>( x => x.Primary ); }
		}

		public static IDelegateWorker Background
		{
			get { return ServiceLocation.With<IDelegateWorkerProvider, IDelegateWorker>( x => x.Secondary ); }
		}
	}

	public class DelegateWorkerProvider : IDelegateWorkerProvider
	{
		readonly IDelegateWorker primary;
		readonly IDelegateWorker secondary;

		public DelegateWorkerProvider( DispatcherDelegateWorker primary, TaskDelegateWorker secondary )
		{
			this.primary = primary;
			this.secondary = secondary;
		}

		public IDelegateWorker Primary
		{
			get { return primary ; }
		}	

		public IDelegateWorker Secondary
		{
			get { return secondary; }
		}
	}

	public interface IDelegateWorkerProvider
	{
		IDelegateWorker Primary { get; }
		IDelegateWorker Secondary { get; }
	}

	public class DispatcherDelegateWorker : IDelegateWorker
	{
		readonly Dispatcher dispatcher;

		public DispatcherDelegateWorker( Dispatcher dispatcher )
		{
			this.dispatcher = dispatcher;
		}

		public IDelegateContext Execute( Action target )
		{
			if ( dispatcher.CheckAccess() )
			{
				target();
			}
			else
			{
				Start( target );
			}
			return null;
		}

		public IDelegateContext Start( Action target )
		{
			dispatcher.BeginInvoke( target );
			return null;
		}

		public IDelegateContext Delay( Action target, TimeSpan time )
		{
			var timer = CreateTimer( target, time );
			timer.Start();
			return null;
		}

		DispatcherTimer CreateTimer( Action target, TimeSpan time )
		{
			var result = new DispatcherTimer( time, DispatcherPriority.Normal, ( s, a ) =>
			{
				s.As<DispatcherTimer>( x => x.Stop() );
				Execute( target );
			}, dispatcher );
			return result;
		}
	}

	public static class DispatcherExtensions
	{
		public static TResult Resolve<TResult>( this Dispatcher @this, Func<TResult> resolver )
		{
			var operation = @this.BeginInvoke( new Func<TResult>( resolver ) );
			operation.Task.Wait();
			var result = (TResult)operation.Result;
			return result;
		}
	}

	public class BusyIndicator : Xceed.Wpf.Toolkit.BusyIndicator
	{
		object FocusedElement { get; set; }

		protected override void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
		{
			if ( IsBusy )
			{
				FocusedElement = FocusManager.GetFocusedElement( this ).As<FrameworkElement>().Transform( x => this.GetParentOfType<ChildWindow>() != null || x.IsInTree( this ) ? x : null );
			}
			else if ( FocusedElement != null )
			{
				FocusedElement.As<Control>( x => Dispatcher.BeginInvoke( new Func<bool>( x.Focus ) )  );
				FocusedElement = null;
			}
			base.OnIsBusyChanged(e);
		}
	}

}
