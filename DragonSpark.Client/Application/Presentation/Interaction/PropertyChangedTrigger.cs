using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class PropertyChangedTrigger : TriggerBase<FrameworkElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.DataContextChanged += AssociatedObjectOnDataContextChanged;
		}

		void AssociatedObjectOnDataContextChanged( object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs )
		{
			Target = AssociatedObject.DataContext.As<INotifyPropertyChanged>();
		}

		
		INotifyPropertyChanged Target
		{
			get { return target; }
			set
			{
				target.NotNull( x => x.PropertyChanged -= XOnPropertyChanged );
				target = value;
				target.NotNull( x => x.PropertyChanged += XOnPropertyChanged );
			}
		}	INotifyPropertyChanged target;

		void XOnPropertyChanged( object sender, PropertyChangedEventArgs propertyChangedEventArgs )
		{
			Properties.Transform( x => x.Contains( propertyChangedEventArgs.PropertyName ), () => true ).IsTrue( () => Threading.Application.Start( () => InvokeActions( propertyChangedEventArgs.PropertyName ) ) );
		}

		[TypeConverter( typeof(StringArrayConverter) )]
		public string[] Properties
		{
			get { return GetValue( PropertiesProperty ).To<string[]>(); }
			set { SetValue( PropertiesProperty, value ); }
		}	public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register( "Properties", typeof(string[]), typeof(PropertyChangedTrigger), null );

	}
}