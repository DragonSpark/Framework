using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime.Values;
using Dynamitey.DynamicObjects;
using Ploeh.AutoFixture;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Testing.Framework.Setup
{
	public class SetupAutoData
	{
		public static SetupAutoData Create( IFixture fixture, MethodBase method )
		{
			var setups = new object[] { fixture, method }.Select( o => new AssociatedSetup( o ) ).ToArray();
			var result = setups.FirstWhere( setup => setup.Item ) ?? new SetupAutoData( fixture, method ).With( context => setups.Each( setup => setup.Assign( context ) ) );
			return result;
		}

		SetupAutoData( [Required]IFixture fixture, [Required]MethodBase method )
		{
			Fixture = fixture;
			Method = method;
		}

		public IFixture Fixture { get; }
		public MethodBase Method { get; }

		public IList<ITestExecutionAware> Items { get; } = new List<ITestExecutionAware>();
	}
}