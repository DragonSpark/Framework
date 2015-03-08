using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Client;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Extensions;
using DragonSpark.Application.Presentation.Interaction;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Fields
{
	public abstract class EntityAssociationField<TAssociation> : ViewObject, IRevertibleChangeTracking where TAssociation : class
	{
		readonly IEntitySetService entitySetService;
		readonly IDomainContextLocator locator;

		protected EntityAssociationField( IEntitySetService entitySetService, IDomainContextLocator locator )
		{
			this.entitySetService = entitySetService;
			this.locator = locator;
		}

		public void Initialize()
		{
			OnInitialize( EventArgs.Empty );
		}

		protected virtual void OnInitialize( EventArgs args )
		{
			Context = EntityType.Transform( locator.Locate );
			Profile = EntityType.Transform( x => entitySetService.RetrieveProfiles().FirstOrDefault( y => y.EntityType == x ) );
			Value = PropertyInfo.GetValue( Owner, null ).To<TAssociation>();
			NotifyOfPropertyChange( () => EntityType );
		}

		public void Refresh()
		{
			this.Validate().IsTrue( () => OnRefresh( EventArgs.Empty ) );
		}

		protected virtual void OnRefresh( EventArgs empty )
		{}

		[Required]
		public System.ServiceModel.DomainServices.Client.Entity Owner
		{
			get { return owner; }
			set { SetProperty( ref owner, value, () => Owner ).IsTrue( Refresh ); }
		}	System.ServiceModel.DomainServices.Client.Entity owner;

		[Required]
		public virtual PropertyInfo PropertyInfo
		{
			get { return propertyInfo; }
			set { SetProperty( ref propertyInfo, value, () => PropertyInfo ); }
		}	PropertyInfo propertyInfo;

		public TAssociation Value
		{
			get { return valueField; }
			protected set { SetProperty( ref valueField, value, () => Value ); }
		}	TAssociation valueField;

		public virtual Type EntityType { get { return Profile.EntityType; } }

		protected IEntitySetService EntitySetService { get { return entitySetService; } }

		internal IEntitySetProfile Profile { get; private set; }

		public DomainContext Context
		{
			get { return context; }
			private set { SetProperty( ref context, value, () => Context ); }
		}	DomainContext context;

		protected virtual void AssignProperties( object item )
		{
			MarkAsModified();
			PropertyInfo.DetermineAssociationProperties().Apply( x =>
			{
			    var value = item.GetType().GetProperty( x.Item2 ).GetValue( item, null );
			    Owner.GetType().GetProperty( x.Item1 ).SetValue( Owner, value, null );
			} );
		}

		protected EntitySet EntitySet
		{
			get { return Context.EntityContainer.GetEntitySet( EntityType ); }
		}

		void IChangeTracking.AcceptChanges()
		{
			OnAcceptChanges();
		}

		protected virtual void OnAcceptChanges()
		{}

		bool IChangeTracking.IsChanged
		{
			get { return EntitySet.HasChanges; }
		}

		void IRevertibleChangeTracking.RejectChanges()
		{
			OnRejectChanges();
		}

		protected virtual void OnRejectChanges()
		{
			EntitySet.To<IRevertibleChangeTracking>().RejectChanges();
		}

		protected void MarkAsModified()
		{
			Owner.As<IModifiable>( x => x.MarkAsModified() );
		}
	}
}