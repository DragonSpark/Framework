using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
	public class EntitySetProfile : Behavior<FrameworkElement>
	{
		[Dependency]
		public IEntitySetService EntitySetService { get; set; }

		protected override void OnAttached()
		{
			this.BuildUpOnce();
			AssociatedObject.EnsureLoaded( x => Refresh() );
			base.OnAttached();
		}

		public object Entity
		{
			get { return GetValue( EntityProperty ).To<object>(); }
			set { SetValue( EntityProperty, value ); }
		}	public static readonly DependencyProperty EntityProperty = DependencyProperty.Register( "Entity", typeof(object), typeof(EntitySetProfile), new PropertyMetadata( OnEntityChanged ) );

		public IEntitySetProfile Profile
		{
			get { return GetValue( ProfileProperty ).To<IEntitySetProfile>(); }
			private set { SetValue( ProfileProperty, value ); }
		}	public static readonly DependencyProperty ProfileProperty = DependencyProperty.Register( "Profile", typeof(IEntitySetProfile), typeof(EntitySetProfile), null );
		
		static void OnEntityChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
		{
			d.As<EntitySetProfile>( x => x.Refresh() );
		}

		void Refresh()
		{
			var entity = Entity ?? AssociatedObject.DataContext;
			var type = entity.Transform( x => x.As<Type>() ?? x.GetType() );
			Profile = entity.Transform( x => EntitySetService.RetrieveProfiles().SingleOrDefault( y => y.EntityType == type ) );
		}
	}

	public class DisplayFieldView : Behavior<DataForm>
	{
		// readonly IDictionary<object, FrameworkElement> cache = new Dictionary<object, FrameworkElement>();
			
		[Dependency]
		public IEntityFieldViewService ViewService { get; set; }

		protected override void OnAttached()
		{
			this.BuildUpOnce();

			AssociatedObject.AutoGeneratingField += AssociatedObjectAutoGeneratingField;
			base.OnAttached();
		}

		void AssociatedObjectAutoGeneratingField( object sender, DataFormAutoGeneratingFieldEventArgs e )
		{
			var fieldView = AssociatedObject.CurrentItem.Transform( x => ViewService.RetrieveView( new EntityField( x.GetType(), FieldViewName, e.PropertyName ) ) );
			e.Cancel = !fieldView.Transform( x => x.IsVisible, () => true );
			e.Cancel.IsFalse( () =>
			{
				e.Field.IsReadOnly = !fieldView.IsEditable;
			    e.Field.PropertyPath = e.PropertyName;
				e.Field.Content = fieldView.Model.Transform( x => new ContentControl { Content = x } ) ?? Process( e.Field.Content );
			} );
		}

		static FrameworkElement Process( FrameworkElement content )
		{
			content.GetAllBindings().Apply( x =>
			{
				var binding = new Binding( x.Item2.ParentBinding ) { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
				content.SetBinding( x.Item1, binding );
			} );
			return content;
		}

		public string FieldViewName
		{
			get { return GetValue( FieldViewNameProperty ).To<string>(); }
			set { SetValue( FieldViewNameProperty, value ); }
		}	public static readonly DependencyProperty FieldViewNameProperty = DependencyProperty.Register( "FieldViewName", typeof(string), typeof(DisplayFieldView), null );

		protected override void OnDetaching()
		{
			AssociatedObject.AutoGeneratingField -= AssociatedObjectAutoGeneratingField;
			base.OnDetaching();
		}
	}
}