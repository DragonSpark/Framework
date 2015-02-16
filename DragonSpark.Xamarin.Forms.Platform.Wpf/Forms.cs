using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xamarin.Forms;
using Expression = System.Linq.Expressions.Expression;
using Size = Xamarin.Forms.Size;

namespace DragonSpark.Xamarin.Forms.Platform.Wpf
{
	public interface IApplicationView
	{
		
	}

	public static class Forms
	{
		static readonly ConditionMonitor Monitor = new ConditionMonitor();

		sealed class DeviceInfo : global::Xamarin.Forms.DeviceInfo
		{
			readonly Size pixelScreenSize;
			readonly Size scaledScreenSize;
			readonly double scalingFactor;

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

			public DeviceInfo()
			{
				var content = System.Windows.Application.Current.MainWindow;
				scalingFactor = 1.0; // (double)content.ScaleFactor;
				pixelScreenSize = new Size( content.ActualWidth * scalingFactor, content.ActualHeight * scalingFactor );
				scaledScreenSize = new Size( content.ActualWidth, content.ActualHeight );
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
						VisitList( listInitExpression.Initializers, delegate( ElementInit initializer ) { VisitList( initializer.Arguments, this.Visit ); } );
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

		/*public static UIElement Render( this Page page, IApplicationView view )
		{
			Monitor.Apply( () =>
			{
				new EventTrigger();
				/*var name = Assembly.GetExecutingAssembly().GetName().Name;
				Application.Current.Resources.MergedDictionaries.Add( new ResourceDictionary
				{
					Source = new Uri( string.Format( "/{0};component/Resources.xaml", name ), UriKind.Relative )
				} );#1#
				var color = global::Xamarin.Forms.Application.Current.Resources["PhoneAccentBrush"].AsTo<SolidColorBrush, Color>( x => x.Color );
				global::Xamarin.Forms.Color.Accent = global::Xamarin.Forms.Color.FromRgba( color.R, color.G, color.B, color.A );
				
				Log.Listeners.Add( new DelegateLogListener( ( c, m ) => Console.WriteLine( @"[{0}] {1}", m, c ) ) );
				Device.OS = TargetPlatform.Other;
				Device.PlatformServices = new PlatformServices();
				Device.Info = new DeviceInfo();
				Device.Idiom = TargetIdiom.Desktop;
				Ticker.Default = new Infrastructure.Ticker();
				global::Xamarin.Forms.ExpressionSearch.Default = new ExpressionSearch();

				var handlers = new[] { typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute) };
				Registrar.RegisterAll( handlers );
			} );

			var result = new Rendering.ApplicationHost( view );
			result.SetPage( page );
			return result;
		}*/
	}
}