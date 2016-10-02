using DragonSpark.TypeSystem;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class AssemblyExtensions
	{
		public static string GetRootNamespace( this Assembly target )
		{
			var types = AssemblyTypes.Public.Get( target );
			var root = target.GetName().Name;
			var result = types.GroupBy( type => type.Namespace ).Select( x => x.Key ).OrderBy( x => x.Length ).FirstOrDefault( x => x.StartsWith( root ) );
			return result;
		}
	}
}
