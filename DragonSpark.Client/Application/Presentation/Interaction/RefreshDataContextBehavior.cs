using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Security.Principal;
using System.Windows;
using DragonSpark.Extensions;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class RefreshDataContextBehavior : PrincipalRefreshedBehaviorBase
	{
		protected override void OnRefresh( IPrincipal principle )
		{
			AssociatedObject.DataContext = DetermineContext();
		}

		public Type ContextType
		{
			get { return GetValue( ContextTypeProperty ).To<Type>(); }
			set { SetValue( ContextTypeProperty, value ); }
		}	public static readonly DependencyProperty ContextTypeProperty = DependencyProperty.Register( "ContextType", typeof(Type), typeof(RefreshDataContextBehavior), null );

		protected virtual object DetermineContext()
		{
			var result = Activator.CreateInstance<object>( ContextType );

			Exports.Locate<CompositionContainer>().SatisfyImportsOnce( result );

			return result;
		}
	}
}