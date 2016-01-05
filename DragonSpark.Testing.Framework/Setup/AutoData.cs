using System;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using PostSharp.Patterns.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AutoDataFactory : FactoryBase<Tuple<IFixture, MethodInfo>, AutoData>
	{
		public static AutoDataFactory Instance { get; } = new AutoDataFactory();

		protected override AutoData CreateItem( Tuple<IFixture, MethodInfo> parameter )
		{
			var setups = new object[] { parameter.Item1, parameter.Item2 }.Select( o => new AssociatedAutoData( o ) ).ToArray();
			var result = new AutoData( parameter.Item1, parameter.Item2 ).With( context => setups.Each( setup => setup.Assign( context ) ) );
			return result;
		}
	}

	public class AutoData : IDisposable
	{
		public AutoData( [Required]IFixture fixture, [Required]MethodBase method )
		{
			Fixture = fixture;
			Method = method;
			Items = new List<IAutoDataCustomization>( new object[] { Fixture, Method }.SelectMany( o => new Items<IAutoDataCustomization>( o ).Item.Purge() ) );

			Items.Each( customization => customization.Initializing( this ) );
		}

		public IFixture Fixture { get; }
		public MethodBase Method { get; }

		IList<IAutoDataCustomization> Items { get; }

		public void Dispose()
		{
			Items.Each( customization => customization.Initialized( this ) );
		}
	}
}