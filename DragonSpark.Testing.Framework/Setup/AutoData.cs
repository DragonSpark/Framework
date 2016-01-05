using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using PostSharp.Patterns.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AutoData
	{
		public static AutoData Create( IFixture fixture, MethodBase method )
		{
			var setups = new object[] { fixture, method }.Select( o => new AssociatedAutoData( o ) ).ToArray();
			var result = setups.FirstWhere( setup => setup.Item ) ?? new AutoData( fixture, method ).With( context => setups.Each( setup => setup.Assign( context ) ) );
			return result;
		}

		AutoData( [Required]IFixture fixture, [Required]MethodBase method )
		{
			Fixture = fixture;
			Method = method;
			Items = new List<IAutoDataCustomization>( new object[] { Fixture, Method }.SelectMany( o => new Items<IAutoDataCustomization>( o ).Item.Purge() ) );
		}

		public IFixture Fixture { get; }
		public MethodBase Method { get; }

		public IList<IAutoDataCustomization> Items { get; }
	}
}