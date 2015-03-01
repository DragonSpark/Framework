using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
    static partial class NotifyChangedExtensions
	{
		readonly static IList<NotificationContext> Contexts = new List<NotificationContext>();

		partial class NotificationContext : IEquatable<NotificationContext>
		{
			readonly IViewObject target;
			readonly INotifyPropertyChanged source;
			readonly INotifyCollectionChanged sourceCollection;
			readonly string name;

			public NotificationContext( IViewObject target, INotifyPropertyChanged source, INotifyCollectionChanged sourceCollection, string name )
			{
				this.sourceCollection = sourceCollection;
				this.target = target;
				this.source = source;
				this.name = name;
			}

			public bool Equals( NotificationContext other )
			{
				if ( ReferenceEquals( null, other ) )
				{
					return false;
				}
				if ( ReferenceEquals( this, other ) )
				{
					return true;
				}
				return Equals( other.target, target ) && Equals( other.source, source ) && Equals( other.sourceCollection, sourceCollection ) && Equals( other.name, name );
			}

			public override bool Equals( object obj )
			{
				if ( ReferenceEquals( null, obj ) )
				{
					return false;
				}
				if ( ReferenceEquals( this, obj ) )
				{
					return true;
				}
				if ( obj.GetType() != typeof(NotificationContext) )
				{
					return false;
				}
				return Equals( (NotificationContext)obj );
			}

			public override int GetHashCode()
			{
				unchecked
				{
					int result = ( target != null ? target.GetHashCode() : 0 );
					result = ( result * 397 ) ^ ( source != null ? source.GetHashCode() : 0 );
					result = ( result * 397 ) ^ ( sourceCollection != null ? sourceCollection.GetHashCode() : 0 );
					result = ( result * 397 ) ^ ( name != null ? name.GetHashCode() : 0 );
					return result;
				}
			}

			public static bool operator ==( NotificationContext left, NotificationContext right )
			{
				return Equals( left, right );
			}

			public static bool operator !=( NotificationContext left, NotificationContext right )
			{
				return !Equals( left, right );
			}

			internal void PropertyChanged( object sender, PropertyChangedEventArgs e )
			{
				target.NotifyOfPropertyChange( name );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Want to ensure a property-expression is used." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used for convenience." )]
		public static TNotifyPropertyChanged Watching<TProperty,TNotifyPropertyChanged>( this IViewObject target, TNotifyPropertyChanged item, System.Linq.Expressions.Expression<Func<TProperty>> property, bool notify = true ) 
			where TNotifyPropertyChanged : class, INotifyPropertyChanged
		{
			var name = property.GetMemberInfo().Name;
			var context = new NotificationContext( target, item, null, name );
			if ( !Contexts.Contains( context ) )
			{
				Contexts.Add( context );
				item.PropertyChanged += context.PropertyChanged;
				notify.IsTrue( () => Threading.Application.Start( () => target.NotifyOfPropertyChange( name ) ) );
			}
			return item;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Want to ensure a property-expression is used." ), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used for convenience.")]
		public static TNotifyPropertyChanged StopWatching<TProperty,TNotifyPropertyChanged>( this IViewObject target, TNotifyPropertyChanged item, System.Linq.Expressions.Expression<Func<TProperty>> property ) 
			where TNotifyPropertyChanged : class, INotifyPropertyChanged
		{
			var name = property.GetMemberInfo().Name;
			var context = new NotificationContext( target, item, null, name );
			if ( Contexts.Contains( context ) )
			{
				Contexts.Remove( context );
				item.PropertyChanged -= context.PropertyChanged;
			}
			return item;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Want to ensure a property-expression is used." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used for convenience." )]
		public static TNotifyCollectionChanged WatchingCollection<TProperty,TNotifyCollectionChanged>( this IViewObject target, TNotifyCollectionChanged item, System.Linq.Expressions.Expression<Func<TProperty>> property ) 
			where TNotifyCollectionChanged : class, INotifyCollectionChanged
		{
			var name = property.GetMemberInfo().Name;
			var context = new NotificationContext( target, null, item, name );
			if ( !Contexts.Contains( context ) )
			{
				Contexts.Add( context );
				item.CollectionChanged += context.CollectionChanged;
			}
			return item;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Want to ensure a property-expression is used." )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used for convenience." )]
		public static TNotifyCollectionChanged StopWatchingCollection<TProperty,TNotifyCollectionChanged>( this IViewObject target, TNotifyCollectionChanged item, System.Linq.Expressions.Expression<Func<TProperty>> property ) 
			where TNotifyCollectionChanged : class, INotifyCollectionChanged
		{
			var name = property.GetMemberInfo().Name;
			var context = new NotificationContext( target, null, item, name );
			if ( Contexts.Contains( context ) )
			{
				Contexts.Remove( context );
				item.CollectionChanged -= context.CollectionChanged;
			}
			return item;
		}
	}
}