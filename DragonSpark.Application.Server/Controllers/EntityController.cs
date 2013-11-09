using System.Linq;
using System.Web.Http;
using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Breeze.WebApi2;
using DragonSpark.Application.Server.Models;
using DragonSpark.Server.ClientHosting;
using Newtonsoft.Json.Linq;

namespace DragonSpark.Application.Server.Controllers
{
    [RoutePrefix( "Entity" ), BreezeController]
	public class EntityController : ApiController
	{
		readonly EFContextProvider<EntityStorage> provider = new EFContextProvider<EntityStorage>();

		[Route( "Metadata" )]
		public string GetMetadata()
		{
			var result = provider.Metadata();
			return result;
		}

		
		[Route( "Profiles" ), EntityQuery]
		public IQueryable<ApplicationUserProfile> GetProfiles()
		{
			return provider.Context.Users.Where( x => x.MembershipNumber.HasValue );
		}

		[Route( "SaveChanges" )]
		public SaveResult SaveChanges( JObject saveBundle )
		{
			var result = provider.SaveChanges( saveBundle );
			return result;
		}
	}
}
