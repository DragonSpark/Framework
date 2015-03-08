using System.ComponentModel;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public abstract class EntityDataBehaviorBase<TEntity> : IEntityDataBehavior where TEntity : Entity
    {
        protected TEntity AssociatedEntity { get; private set; }

        void IEntityDataBehavior.Attach( object entity )
        {
            entity.As<TEntity>( x =>
            {
                AssociatedEntity = x;
                Attach( x );
            } );
        }

        protected virtual void Attach( TEntity entity )
        {
            entity.PropertyChanged += EntityOnPropertyChanged;
        }

        void IEntityDataBehavior.Detach()
        {
            Detach();
            AssociatedEntity = null;
        }

        protected virtual void Detach()
        {
            AssociatedEntity.PropertyChanged -= EntityOnPropertyChanged;
        }

        public string[] Properties
        {
            get { return properties ?? ( properties = ResolveProperties() ); }
        }	string[] properties;

        protected virtual string[] ResolveProperties()
        {
            return null;
        }

        void EntityOnPropertyChanged( object sender, PropertyChangedEventArgs propertyChangedEventArgs )
        {
            if ( Properties == null || Properties.Contains( propertyChangedEventArgs.PropertyName ) )
            {
                Apply( propertyChangedEventArgs.PropertyName );
            }
        }

        protected abstract void Apply( string propertyName );

    }
}