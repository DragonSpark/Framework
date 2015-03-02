using DragonSpark.Application.Client.Commanding;
using DragonSpark.Application.Client.Extensions;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism;
using PostSharp.Patterns.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using Expression = System.Linq.Expressions.Expression;
using ICommand = DragonSpark.Application.Client.Commanding.ICommand;

namespace DragonSpark.Application.Client.Presentation
{
	public class ViewCollection<T> : ObservableCollection<T>, IObservableCollection<T>
	{
		readonly Collection<T> adding = new Collection<T>();
		readonly Collection<T> removing = new Collection<T>();

		public ViewCollection()
		{}

		public ViewCollection( IEnumerable<T> collection ) : base( collection )
		{}

		public new IEnumerator<T> GetEnumerator()
		{
			var result = adding.ToArray().Concat( Items.ToArray() ).Except( removing.ToArray() ).ToList().GetEnumerator();
			return result;
		}

		public bool EnableNotifications
		{
			get { return enableNotifications; }
			set { enableNotifications = value; }
		}	bool enableNotifications = true;

		public void NotifyOfPropertyChange( string propertyName )
		{
			if ( EnableNotifications )
			{
				RaisePropertyChangedEventImmediately( new PropertyChangedEventArgs( propertyName ) );
			}
		}

		public void RefreshAllNotifications()
		{
			OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
			OnPropertyChanged( new PropertyChangedEventArgs( string.Empty ) );
		}

		protected override sealed void InsertItem( int index, T item )
		{
			adding.Add( item );
			InsertItemBase( index, item );
		}

		[Dispatched]
		protected virtual void InsertItemBase( int index, T item )
		{
			adding.Remove( item );
			base.InsertItem( index, item );
		}

		/// <summary>
		///     Sets the item at the specified position.
		/// </summary>
		/// <param name="index">The index to set the item at.</param>
		/// <param name="item">The item to set.</param>
		protected override sealed void SetItem( int index, T item )
		{
			SetItemBase( index, item );
		}

		[Dispatched]
		protected virtual void SetItemBase( int index, T item )
		{
			base.SetItem( index, item );
		}

		protected override sealed void RemoveItem( int index )
		{
			var item = Items.ElementAtOrDefault( index );
			item.NotNull( removing.Add );
			RemoveItemBase( index );
		}

		[Dispatched]
		protected virtual void RemoveItemBase( int index )
		{
			var item = Items.ElementAtOrDefault( index );
			item.NotNull( x => removing.Remove( x ) );
			base.RemoveItem( index );
		}

		protected override sealed void ClearItems()
		{
			removing.AddRange( Items );
			ClearItemsBase();
		}

		[Dispatched]
		protected virtual void ClearItemsBase()
		{
			removing.Clear();
			base.ClearItems();
		}

		[Dispatched]
		protected override void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
		{
			if ( EnableNotifications )
			{
				base.OnCollectionChanged( e );
			}
		}

		[Dispatched]
		protected override void OnPropertyChanged( PropertyChangedEventArgs e )
		{
			if ( EnableNotifications )
			{
				base.OnPropertyChanged( e );
			}
		}

		void RaisePropertyChangedEventImmediately( PropertyChangedEventArgs e )
		{
			OnPropertyChanged( e );
		}

		[Dispatched]
		public virtual void AddRange( IEnumerable<T> items )
		{
			var range = items.ToArray();
			adding.AddRange( range );
			var previousNotificationSetting = EnableNotifications;
			EnableNotifications = false;
			var index = Count;
			foreach ( var item in range )
			{
				InsertItemBase( index, item );
				index++;
			}
			EnableNotifications = previousNotificationSetting;
			OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
			OnPropertyChanged( new PropertyChangedEventArgs( string.Empty ) );
		}

		[Dispatched]
		public virtual void RemoveRange( IEnumerable<T> items )
		{
			var range = items.ToArray();
			removing.AddRange( range );
			var previousNotificationSetting = EnableNotifications;
			EnableNotifications = false;
			foreach ( var item in range )
			{
				var index = IndexOf( item );
				RemoveItemBase( index );
			}
			EnableNotifications = previousNotificationSetting;
			OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
			OnPropertyChanged( new PropertyChangedEventArgs( string.Empty ) );
		}

		protected override sealed void MoveItem( int oldIndex, int newIndex )
		{
			MoveItemBase( oldIndex, newIndex );
		}

		[Dispatched]
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
		bool EnableNotifications { get; }

		void NotifyOfPropertyChange( string propertyName );

		void RefreshAllNotifications();
	}

	public static class PropertySupport
	{
		public static bool SetProperty<TItem>( ref TItem current, TItem assignment, Expression expression, Action<string> notify )
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
		public event EventHandler Attached = delegate { } , Detached = delegate { };

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
		public ViewObject()
		{
			EnableNotifications = true;
		}

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public bool EnableNotifications { get; set; }

		public void RefreshAllNotifications()
		{
			NotifyOfPropertyChange( string.Empty );

			UpdateCommands();
		}

		protected virtual void UpdateCommands()
		{
			this.GetAllPropertyValuesOf<ICommand>().Apply( x => x.Update() );
		}

		[SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Used as convenience." ), SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract expression name." )]
		protected bool SetProperty<TItem>( ref TItem current, TItem assignment, Expression<Func<TItem>> expression )
		{
			var result = PropertySupport.SetProperty( ref current, assignment, expression, NotifyOfPropertyChange );
			return result;
		}

		[SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Used as convenience." ), SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract expression name." )]
		protected bool SetProperty<TItem>( ref TItem current, TItem assignment, [CallerMemberName] string propertyName = null )
		{
			var result = PropertySupport.SetProperty( ref current, assignment, propertyName, NotifyOfPropertyChange );
			return result;
		}

		public virtual void NotifyOfPropertyChange( string propertyName )
		{
			if ( EnableNotifications )
			{
				RaisePropertyChangedEventCore( propertyName );
			}
		}

		[SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract expression." )]
		public virtual void NotifyOfPropertyChange<TProperty>( Expression<Func<TProperty>> expression )
		{
			NotifyOfPropertyChange( expression.GetMemberInfo().Name );
		}

		[SuppressMessage( "Microsoft.Design", "CA1030:UseEventsWhereAppropriate" )]
		protected virtual void RaisePropertyChangedEventImmediately( string propertyName )
		{
			if ( EnableNotifications )
			{
				RaisePropertyChangedEventCore( propertyName );
			}
		}

		[Dispatched]
		void RaisePropertyChangedEventCore( string propertyName )
		{
			PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
		}
	}

	public class BusyIndicator : Xceed.Wpf.Toolkit.BusyIndicator
	{
		object FocusedElement { get; set; }

		protected override void OnIsBusyChanged( DependencyPropertyChangedEventArgs e )
		{
			if ( IsBusy )
			{
				FocusedElement = FocusManager.GetFocusedElement( this ).As<FrameworkElement>().Transform( x => this.GetParentOfType<ChildWindow>() != null || x.IsInTree( this ) ? x : null );
			}
			else if ( FocusedElement != null )
			{
				FocusedElement.As<Control>( x => Dispatcher.BeginInvoke( new Func<bool>( x.Focus ) ) );
				FocusedElement = null;
			}
			base.OnIsBusyChanged( e );
		}
	}
}