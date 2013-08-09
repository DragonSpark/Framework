using System.Data.Entity;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using DragonSpark.Application.Communication.Entity;

namespace DragonSpark.Application.Communication.Security
{
    public abstract class SecurityServiceBase<TStorage, TUser> : DbDomainService<TStorage> where TStorage : DbContext, new() where TUser : class
    {
        [RequiresRole( "Administrator" )]
        public void Add( TUser entity ) { UpdateUser( entity ); }

        public void UpdateUser( TUser user )
        {
            DbContext.ApplyChanges( user );
        }

        [RequiresRole( "Administrator" )]
        public void Remove( TUser entity )
        {
            DbContext.Remove( entity );
        }

        public IQueryable<TUser> RetrieveUsers( string filter )
        {
            var result = ResolveUserQuery().Query( filter );
            return result;
        }

        public void Add( Role entity ) { Update( entity ); }
        public void Update( Role entity )
        {
            DbContext.ApplyChanges( entity );
        }

        public void Remove( Role entity )
        {
            DbContext.Remove( entity );
        }

        public IQueryable<Role> RetrieveRoles( string filter )
        {
            var result = DbContext.Set<Role>().Query( filter );
            return result;
        }
		
        /*[Invoke, RequiresRole( "Administrator" )]
		public void SaveUser( TUser user )
		{
			DbContext.ApplyChanges( user );
			DbContext.SaveChanges();
		}*/

        protected virtual IQueryable<TUser> ResolveUserQuery()
        {
            return DbContext.Set<TUser>();
        }
    }
}