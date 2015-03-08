using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class ParentElement : Behavior<FrameworkElement>
	{
		protected override void OnAttached()
		{
			AssociatedObject.EnsureLoaded( y =>
			{
			    Element = ElementType.Transform( x => AssociatedObject.GetParentOfType( x ).To<FrameworkElement>(), ResolveParent );
			} );
			base.OnAttached();
		}

		FrameworkElement ResolveParent()
		{
			var result = AssociatedObject.GetParentElementContaining<FrameworkElement>( FrameworkElement.DataContextProperty, false );
			return result;
		}

		public FrameworkElement AttachedElement
		{
			get { return AssociatedObject; }
		}

		public FrameworkElement Element
		{
			get { return GetValue( ParentElementProperty ).To<FrameworkElement>(); }
			private set { SetValue( ParentElementProperty, value ); }
		}	public static readonly DependencyProperty ParentElementProperty = DependencyProperty.Register( "Element", typeof(FrameworkElement), typeof(ParentElement), null );

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type ElementType
		{
			get { return GetValue( ElementTypeProperty ).To<Type>(); }
			set { SetValue( ElementTypeProperty, value ); }
		}	public static readonly DependencyProperty ElementTypeProperty = DependencyProperty.Register( "ElementType", typeof(Type), typeof(ParentElement), null );
	}
}