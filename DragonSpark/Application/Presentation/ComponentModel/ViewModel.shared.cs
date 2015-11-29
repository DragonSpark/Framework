using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public class ViewModel<TSource> : DynamicObject, IViewObject where TSource : class
	{
		static readonly MethodInfo GetDefaultMethod = typeof(ViewModel<TSource>).GetMethod( "GetDefault", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance );

		readonly Dictionary<string,object> properties = new Dictionary<string, object>();

		public ViewModel() : this( default(TSource) )
		{
		}

		public ViewModel( TSource source )
		{
			this.source = source;
			IsNotifying = true;
		}

		public TSource Source
		{
			get { return source; }
			set
			{
				if ( source != value )
				{
					source = value;
					NotifyOfPropertyChange( "Source" );
				}
			}
		}

		TSource source;

		/*public string DisplayName
		{
			get { return displayName ?? ( displayName = ResolveDisplayName() ); }
		}	string displayName;

		string ResolveDisplayName()
		{
			var result = Source.FromMetadata<EntitySetProfileAttribute, string>( item => item.DisplayNamePath.Transform( i => Objects.Synchronization.Expression.EvaluateValue( Source, i ).Transform( value => value.ToString() ) ), Source.ToString );
			return result;
		}*/

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			var result = source.GetType().GetProperties().Concat( GetType().GetProperties() ).Select( x => x.Name ).Concat( properties.Keys );
			return result;
		}

		public virtual event PropertyChangedEventHandler PropertyChanged = delegate { };

		/// <summary>
		/// Notifies subscribers of the property change.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		public virtual void NotifyOfPropertyChange(string propertyName)
		{
			if ( IsNotifying )
			{
				Threading.Application.Execute( () => RaisePropertyChangedEventImmediately( propertyName ) );
			}
		}

		public void RefreshAllNotifications()
		{
			NotifyOfPropertyChange(string.Empty);
		}

		public bool IsNotifying { get; set; }

		/// <summary>
		/// Notifies subscribers of the property change.
		/// </summary>
		/// <param name="propertyExpression">The property expression.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to determine name of property to fire event on.")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Want to ensure a property-expression is used." )]
		public void NotifyOfPropertyChange<TProperty>( System.Linq.Expressions.Expression<Func<TProperty>> property )
		{
			NotifyOfPropertyChange( property.GetMemberInfo().Name );
		}

		/// <summary>
		/// Raises the property changed event immediately.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		void RaisePropertyChangedEventImmediately(string propertyName)
		{
			if ( IsNotifying )
			{
				PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
			}
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			lock ( properties )
			{
				/*if ( !Set( source, binder, value ) && !Set( this, binder, value ) )
				{
					properties[ binder.Name ] = value;
				}*/

				NotifyOfPropertyChange( binder.Name );
				return true;
			}
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			lock ( properties )
			{
				var success = 
					properties.TryGetValue( binder.Name, out result ) || 
					Get( source, binder, out result ) || 
					Get( this, binder, out result ) ||
					base.TryGetMember( binder, out result ) || EnsureValue( binder, out result );
				return success;
			}
		}

		static TResult GetDefault<TResult>()
		{
			return default(TResult);
		}

		bool EnsureValue( GetMemberBinder binder, out object result )
		{
			result = null;
			properties.Ensure( binder.Name, x => GetDefaultMethod.MakeGenericMethod( binder.ReturnType ).Invoke( null, null ) );
			return true;
		}

		static bool Get( object target, GetMemberBinder binder, out object result )
		{
			result = null;
			var propertyInfo = target.GetType().GetProperty( binder.Name );
			if ( propertyInfo != null && propertyInfo.CanRead )
			{
				try
				{
					result = propertyInfo.GetValue( target, null );
					return true;
				}
				catch ( ArgumentException )
				{
					return false;
				}
			}
			return false;
		}

		/*static bool Set( object target, SetMemberBinder binder, object value )
		{
			target.GetType().GetProperty( binder.Name ).NotNull(x =>
			{
			    // Do some code...
			});
			return false;
		}*/

		public override bool Equals(object obj)
		{
			return !ReferenceEquals( null, obj ) &&
			       ( ReferenceEquals( this, obj ) || obj.GetType() == typeof(ViewModel<TSource>) && Equals( (ViewModel<TSource>)obj ) );
		}

		public bool Equals( ViewModel<TSource> other )
		{
			return !ReferenceEquals( null, other ) && ( ReferenceEquals( this, other ) || Equals( other.source, source ) );
		}

		public override int GetHashCode()
		{
			return source.GetHashCode();
		}
	}
}