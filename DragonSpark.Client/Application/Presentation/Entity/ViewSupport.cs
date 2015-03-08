using System;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Controls;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation.Entity
{
    [Singleton( typeof(IViewSupport), Priority = Priority.Lowest )]
    public class ViewSupport : IViewSupport
    {
        readonly IEntitySetService entitySetService;

        static readonly Tuple<EntitySetOperations, DataFormCommandButtonsVisibility, Func<EntitySet, bool>>[] Mappings = new[]
            {
                new Tuple<EntitySetOperations, DataFormCommandButtonsVisibility, Func<EntitySet, bool>>( EntitySetOperations.Edit, DataFormCommandButtonsVisibility.Commit, x => x.CanEdit ),
                new Tuple<EntitySetOperations, DataFormCommandButtonsVisibility, Func<EntitySet, bool>>( EntitySetOperations.Add, DataFormCommandButtonsVisibility.Add, x => x.CanAdd ),
                new Tuple<EntitySetOperations, DataFormCommandButtonsVisibility, Func<EntitySet, bool>>( EntitySetOperations.Remove, DataFormCommandButtonsVisibility.Delete, x => x.CanRemove )
            };

        public ViewSupport( IEntitySetService entitySetService )
        {
            this.entitySetService = entitySetService;
        }

        public DataFormCommandButtonsVisibility DetermineVisibility( DomainContext context, Type entityType )
        {
            var result = DataFormCommandButtonsVisibility.None;
            var profile = entitySetService.RetrieveProfiles().FirstOrDefault( x => x.EntityType == entityType );
            profile.NotNull( x => Mappings.Apply( y =>
            {
                var entitySet = context.DetermineEntitySet( entityType );
                result |= y.Item3( entitySet ) && entitySetService.IsAuthorized( x, y.Item1 ) ? y.Item2 : DataFormCommandButtonsVisibility.None;
            } ) );
            return result;
        }
    }
}