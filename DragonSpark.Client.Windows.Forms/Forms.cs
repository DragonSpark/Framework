using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;
using Application = System.Windows.Application;
using Expression = System.Linq.Expressions.Expression;
using Size = Xamarin.Forms.Size;

namespace DragonSpark.Client.Windows.Forms
{
	public interface INavigationModel
	{
		void PopToRoot( Page ancestralNav );
		void PushModal( Page page );
		Page PopModal();
		bool RemovePage( Page page );
		void InsertPageBefore( Page page, Page before );
		Page CurrentPage { get; }
		IReadOnlyList<IReadOnlyList<Page>> Tree { get; }
		IEnumerable<Page> Roots { get; }
		void Push( Page root, Page ancester );
		Page Pop( Page ancestor );
	}

	class NavigationModel : Xamarin.Forms.NavigationModel, INavigationModel
	{}

	public static class Compensations
	{
		public static void Assign( this Page @this, IPlatform item )
		{
			@this.Platform = item;
		}

		public static void AssignNavigation( this Page @this, INavigation navigation )
		{
			@this.NavigationProxy.Inner = navigation;
		}

		public static INavigation GetNavigation( this Page @this )
		{
			return @this.NavigationProxy.Inner;
		}
	}

	public interface IInitializer
	{
		void Initialize();
	}

	public static class ShellProperties
	{
		public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached( "Title", typeof(string), typeof(ShellProperties), new PropertyMetadata( OnTitlePropertyChanged ) );

		static void OnTitlePropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		public static string GetTitle( FrameworkElement element )
		{
			return (string)element.GetValue( TitleProperty );
		}

		public static void SetTitle( FrameworkElement element, string value )
		{
			element.SetValue( TitleProperty, value );
		}

		public static readonly DependencyProperty DialogProperty = DependencyProperty.RegisterAttached( "Dialog", typeof(UIElement), typeof(ShellProperties), new PropertyMetadata( OnDialogPropertyChanged ) );

		static void OnDialogPropertyChanged( DependencyObject o, DependencyPropertyChangedEventArgs e )
		{}

		public static Window GetDialog( UIElement element )
		{
			return (Window)element.GetValue( DialogProperty );
		}

		public static void SetDialog( UIElement element, Window value )
		{
			element.SetValue( DialogProperty, value );
		}
	}

	sealed class DeviceInfo : Xamarin.Forms.DeviceInfo
	{
		Size pixelScreenSize, scaledScreenSize;
		double scalingFactor;

		public DeviceInfo()
		{
			if ( !Assign() )
			{
				MessagingCenter.Subscribe<Application>( this, "DragonSpark.Application.Setup.Shell.Initialized", OnReady );
			}
		}

		void OnReady( Application sender )
		{
			Assign();

			Task.Run( () => MessagingCenter.Unsubscribe<Application>( this, "DragonSpark.Application.Setup.Shell.Initialized" ) );
		}

		bool Assign()
		{
			var application = System.Windows.Application.Current;
			var content = application.MainWindow;
			scalingFactor = 1.0; // (double)content.ScaleFactor;
			pixelScreenSize = content.Transform( x => new Size( x.ActualWidth * scalingFactor, x.ActualHeight * scalingFactor ) );
			scaledScreenSize = content.Transform( x => new Size( x.ActualWidth,  x.ActualHeight ) );
			return content != null;
		}

		public override Size PixelScreenSize
		{
			get { return pixelScreenSize; }
		}

		public override Size ScaledScreenSize
		{
			get { return scaledScreenSize; }
		}

		public override double ScalingFactor
		{
			get { return scalingFactor; }
		}
	}

	sealed class ExpressionSearch : IExpressionSearch
	{
		List<object> results;
		Type targeType;

		public List<T> FindObjects<T>( Expression expression ) where T : class
		{
			results = new List<object>();
			targeType = typeof(T);
			Visit( expression );
			return (
				from o in results
				select o as T ).ToList<T>();
		}

		void Visit( Expression expression )
		{
			if ( expression == null )
			{
				return;
			}
			switch ( expression.NodeType )
			{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.AndAlso:
				case ExpressionType.ArrayIndex:
				case ExpressionType.Coalesce:
				case ExpressionType.Divide:
				case ExpressionType.Equal:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LeftShift:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.NotEqual:
				case ExpressionType.Or:
				case ExpressionType.OrElse:
				case ExpressionType.Power:
				case ExpressionType.RightShift:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
				{
					var binaryExpression = (BinaryExpression)expression;
					Visit( binaryExpression.Left );
					Visit( binaryExpression.Right );
					Visit( binaryExpression.Conversion );
					return;
				}
				case ExpressionType.ArrayLength:
				case ExpressionType.Convert:
				case ExpressionType.ConvertChecked:
				case ExpressionType.Negate:
				case ExpressionType.UnaryPlus:
				case ExpressionType.NegateChecked:
				case ExpressionType.Not:
				case ExpressionType.Quote:
				case ExpressionType.TypeAs:
					Visit( ( (UnaryExpression)expression ).Operand );
					return;
				case ExpressionType.Call:
				{
					var methodCallExpression = (MethodCallExpression)expression;
					Visit( methodCallExpression.Object );
					VisitList( methodCallExpression.Arguments, Visit );
					return;
				}
				case ExpressionType.Conditional:
				{
					var conditionalExpression = (ConditionalExpression)expression;
					Visit( conditionalExpression.Test );
					Visit( conditionalExpression.IfTrue );
					Visit( conditionalExpression.IfFalse );
					return;
				}
				case ExpressionType.Constant:
					return;
				case ExpressionType.Invoke:
				{
					var invocationExpression = (InvocationExpression)expression;
					VisitList( invocationExpression.Arguments, Visit );
					Visit( invocationExpression.Expression );
					return;
				}
				case ExpressionType.Lambda:
					Visit( ( (LambdaExpression)expression ).Body );
					return;
				case ExpressionType.ListInit:
				{
					var listInitExpression = (ListInitExpression)expression;
					VisitList( listInitExpression.NewExpression.Arguments, Visit );
					VisitList( listInitExpression.Initializers, initializer => VisitList( initializer.Arguments, Visit ) );
					return;
				}
				case ExpressionType.MemberAccess:
					VisitMemberAccess( (MemberExpression)expression );
					return;
				case ExpressionType.MemberInit:
				{
					var memberInitExpression = (MemberInitExpression)expression;
					VisitList( memberInitExpression.NewExpression.Arguments, Visit );
					VisitList( memberInitExpression.Bindings, VisitBinding );
					return;
				}
				case ExpressionType.New:
					VisitList( ( (NewExpression)expression ).Arguments, Visit );
					return;
				case ExpressionType.NewArrayInit:
				case ExpressionType.NewArrayBounds:
					VisitList( ( (NewArrayExpression)expression ).Expressions, Visit );
					return;
				case ExpressionType.TypeIs:
					Visit( ( (TypeBinaryExpression)expression ).Expression );
					return;
			}
			throw new ArgumentException( string.Format( "Unhandled expression type: '{0}'", expression.NodeType ) );
		}

		void VisitBinding( MemberBinding binding )
		{
			switch ( binding.BindingType )
			{
				case MemberBindingType.Assignment:
					Visit( ( (MemberAssignment)binding ).Expression );
					return;
				case MemberBindingType.MemberBinding:
					VisitList( ( (MemberMemberBinding)binding ).Bindings, VisitBinding );
					return;
				case MemberBindingType.ListBinding:
					VisitList( ( (MemberListBinding)binding ).Initializers, delegate( ElementInit initializer ) { VisitList( initializer.Arguments, this.Visit ); } );
					return;
				default:
					throw new ArgumentException( string.Format( "Unhandled binding type '{0}'", binding.BindingType ) );
			}
		}

		void VisitMemberAccess( MemberExpression member )
		{
			if ( member.Expression is ConstantExpression && member.Member is FieldInfo )
			{
				var value = ( (ConstantExpression)member.Expression ).Value;
				var value2 = ( (FieldInfo)member.Member ).GetValue( value );
				if ( targeType.IsInstanceOfType( value2 ) )
				{
					results.Add( value2 );
				}
			}
			Visit( member.Expression );
		}

		static void VisitList<TList>( IEnumerable<TList> list, Action<TList> visitor )
		{
			foreach ( var current in list )
			{
				visitor( current );
			}
		}
	}
}