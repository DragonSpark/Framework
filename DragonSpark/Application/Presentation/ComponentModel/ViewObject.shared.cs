using System;
using System.ComponentModel;
using System.Linq.Expressions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
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
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Used as convenience." ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract expression name." )]
        protected bool SetProperty<TItem>( ref TItem current, TItem assignment, Expression<Func<TItem>> expression )
		{
			var result = PropertySupport.SetProperty( ref current, assignment, expression, NotifyOfPropertyChange );
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
}