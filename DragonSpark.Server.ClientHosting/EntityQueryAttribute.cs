using System;
using Breeze.WebApi2;

namespace DragonSpark.Server.ClientHosting
{
	[AttributeUsage( AttributeTargets.Method )]
	public class EntityQueryAttribute : BreezeQueryableAttribute
	{
		public string EntityName { get; set; }

		/*protected override Breeze.WebApi.QueryHelper NewQueryHelper()
		{
			return new QueryHelper( GetODataQuerySettings() );
		}*/
	}
}