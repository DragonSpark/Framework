using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.DomainServices.Server.ApplicationServices;
using DragonSpark.Extensions;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Activator = DragonSpark.Runtime.Activator;

namespace DragonSpark.Application.Communication.Entity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Db" )]
    public class DbDomainService<TStorage> : System.ServiceModel.DomainServices.EntityFramework.DbDomainService<TStorage> where TStorage : DbContext, new()
	{
		protected override TStorage CreateDbContext()
		{
		    var result = Activator.Create<TStorage>();
		    return result;
		}

        protected override void OnError( DomainServiceErrorInfo errorInfo )
		{
			#if DEBUG
			Debugger.Break();
			#endif

			Exception error;
			errorInfo.Error = ExceptionPolicy.HandleException( errorInfo.Error, ExceptionShielding.DefaultExceptionPolicy, out error ) ? error : null;
			
			base.OnError( errorInfo );
		}

		protected virtual TUser GetCurrentUser<TUser>() where TUser : class, IUser
		{
			var type = DbContext.GetType();
			var query = type.GetProperties().FirstOrDefault( x => typeof(DbSet<TUser>).IsAssignableFrom( x.PropertyType ) ).Transform( x => x.GetValue( DbContext, null ).To<DbSet<TUser>>() );
			var result = query.SingleOrDefault( x => x.Name == ServiceContext.User.Identity.Name ).To<TUser>();
			return result;
		}
	}
}