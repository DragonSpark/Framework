namespace DragonSpark.Server.ClientHosting
{
	/*public class QueryHelper : Breeze.WebApi.QueryHelper
	{
		static readonly MethodInfo CreateInfo = typeof(QueryHelper).GetMethod( "Create", DragonSparkBindingOptions.AllProperties );

		public QueryHelper( ODataQuerySettings querySettings ) : base( querySettings )
		{}

		static IEnumerable<TItem> Create<TItem>( IEnumerable source )
		{
			var result = source.Cast<TItem>().ToList();
			return result;
		}

		/// <summary>
		///     Perform any work after the query is executed.  Does nothing in this implementation but is available to derived
		///     classes.
		/// </summary>
		/// <param name="queryResult"></param>
		/// <returns></returns>
		public override IEnumerable PostExecuteQuery( IEnumerable queryResult )
		{
			var list = queryResult.Cast<object>().Select( x => x.GetType().GetProperty( "Instance" ).Transform( y => y.GetValue( x, null ), () => x ) ).ToArray();

			var type = list.FirstOrDefault().Transform( x => x.GetType() ) ?? typeof(object);

			var result = (IEnumerable)CreateInfo.MakeGenericMethod( type ).Invoke( null, new object[] { list } );
			return result;
		}

		public override IQueryable ApplySelectAndExpand( IQueryable queryable, NameValueCollection map )
		{
			return queryable;
		}
	}*/

	public class EntityExtensionsBuilder : ClientModuleBuilder
	{
		protected override bool IsResource( string parameter, AssemblyResource resource )
		{
			var result = IsResource( resource, "entityextensions" );
			return result;
		}
	}
}