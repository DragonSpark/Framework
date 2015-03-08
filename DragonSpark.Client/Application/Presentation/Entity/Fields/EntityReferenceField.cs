using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Commands;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public class EntityReferenceField : EntityAssociationField<System.ServiceModel.DomainServices.Client.Entity>
	{
		public EntityReferenceField( IEntitySetService entitySetService, IDomainContextLocator locator ) : base( entitySetService, locator )
		{}

		[Required]
		public ICollectionView View
		{
			get { return view; }
			set { SetProperty( ref view, value, () => View ); }
		}	ICollectionView view;

		public AssociationQueryShaper Shaper
		{
			get { return shaper; }
			private set { SetProperty( ref shaper, value, () => Shaper ); }
		}	AssociationQueryShaper shaper;

		protected override void OnInitialize( EventArgs args )
		{
			base.OnInitialize( args );
			Shaper = new AssociationQueryShaper( Owner, PropertyInfo );
		}

		public bool RequiresLoad
		{
			get { return Value == null && Owner.Transform( x => PropertyInfo.Transform( y => y.FromMetadata<AssociationAttribute, bool>( z => z.ThisKeyMembers.Select( a => x.GetType().GetProperty( a ).GetValue( Owner, null ) ).All( b => !b.IsDefault() ) ) ) ); }
		}

		public EntityTitleMonitor Monitor
		{
			get { return monitor; }
			private set
			{
				if ( monitor != value )
				{
					monitor.TryDispose();
					monitor = value.Transform( x => this.Watching( x, () => Monitor ) );
					monitor.Null( () => NotifyOfPropertyChange( () => Monitor ) );
				}
			}
		}	EntityTitleMonitor monitor;

		protected override void AssignProperties( object item )
		{
			base.AssignProperties( item );
			PropertyInfo.SetValue( Owner, item, null );
		}

		public override Type EntityType
		{
			get { return PropertyInfo.Transform( x => x.PropertyType ); }
		}

		protected override void OnRefresh( EventArgs empty )
		{
			var value = PropertyInfo.GetValue( Owner, null );
			Value = value.To<System.ServiceModel.DomainServices.Client.Entity>();
			Monitor = Value.Transform( x => Profile.Transform( y => new EntityTitleMonitor( y, x ) ) );
			View.MoveCurrentTo( Value );
			NotifyOfPropertyChange( () => RequiresLoad );
		}

		protected override void OnRejectChanges()
		{
			base.OnRejectChanges();
			Refresh();
		}

		internal void Add( System.ServiceModel.DomainServices.Client.Entity item )
		{
			// Update view:
			var entitySet = Context.DetermineEntitySet( item.GetType() );
			entitySet.Add( item );

			View.As<IAppendSource>( x => x.Append( item.ToEnumerable() ) );

			AssignProperties( item );

			Refresh();
		}

		internal void ApplyEdit()
		{
			Refresh();
			MarkAsModified();
		}
		
		public ICommand ClearCommand
		{
			get { return clearCommand ?? ( clearCommand = new DelegateCommand( Clear, () => this.Validate() && Value != null ) ); }
		}	ICommand clearCommand;

		void Clear()
		{
			var properties = PropertyInfo.FromMetadata<AssociationAttribute, IEnumerable<PropertyInfo>>( x => x.ThisKeyMembers.Select( y => PropertyInfo.DeclaringType.GetProperty( y ) ) );
			properties.Apply( x => x.SetValue( Owner, null, null ) );
			Refresh();
		}

		public ICommand DeleteCommand
		{
			get { return deleteCommand ?? ( deleteCommand = new DelegateCommand( Delete, () => this.Validate() && Value != null ) ); }
		}	ICommand deleteCommand;

		void Delete()
		{
			var entitySet = Context.DetermineEntitySet( EntityType );
			entitySet.Remove( Value );
			Clear();
		}

		internal void Assign( System.ServiceModel.DomainServices.Client.Entity selectedEntity )
		{
			AssignProperties( selectedEntity );
			Refresh();
		}
	}
}