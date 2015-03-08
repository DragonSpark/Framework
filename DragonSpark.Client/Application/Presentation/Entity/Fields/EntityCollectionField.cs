using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
    public class EntityCollectionField : EntityAssociationField<ICollectionViewFactory>
	{
		readonly IViewSupport viewSupport;

		public EntityCollectionField( IEntitySetService entitySetService, IViewSupport viewSupport, IDomainContextLocator locator ) : base( entitySetService, locator )
		{
			this.viewSupport = viewSupport;
		}

		internal void Assign( ICollectionView view )
		{
			view.As<IEditableCollectionView>( x => x.CommitEdit() );
			MarkAsModified();
			view.Cast<object>().Apply( AssignProperties );
		}

		public DataFormCommandButtonsVisibility ButtonsVisibility
		{
			get { return viewSupport.DetermineVisibility( Context, EntityType ); }
		}

		protected override void AssignProperties( object item )
		{
			PropertyInfo.DetermineAssociationProperties().Apply( x =>
			{
				var value = Owner.GetType().GetProperty( x.Item1 ).GetValue( Owner, null );
				item.GetType().GetProperty( x.Item2 ).SetValue( item, value, null );
			} );
		}

		public override Type EntityType
		{
			get
			{
				var arguments = PropertyInfo.PropertyType.GetGenericArguments();
				var type = arguments.FirstOrDefault();
				return type;
			}
		}
	}
}