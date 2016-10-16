using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Linq;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public sealed class EqualityReference<T> : AlterationBase<T> where T : class
	{
		public static EqualityReference<T> Default { get; } = new EqualityReference<T>();
		EqualityReference() {}

		readonly WeakList<T> list = new WeakList<T>();

		public override T Get( T parameter )
		{
			lock ( list )
			{
				var current = list.Introduce( parameter, tuple => tuple.Item1.Equals( tuple.Item2 ) ).SingleOrDefault();
				if ( current == null )
				{
					list.Add( parameter );
					return parameter;
				}
				return current;
			}
		}
	}
}