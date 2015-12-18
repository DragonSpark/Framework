using System;
using DragonSpark.Extensions;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public class TypesCustomization : ICustomization
	{
		readonly TypesFactory<ICustomization> factory;
		readonly Type[] types;

		public TypesCustomization( Type[] types ) : this( TypesFactory<ICustomization>.Instance, types )
		{}

		public TypesCustomization( TypesFactory<ICustomization> factory, Type[] types )
		{
			this.factory = factory;
			this.types = types;
		}

		public void Customize( IFixture fixture )
		{
			using ( FixtureContext.Create( fixture ) )
			{
				var customizations = factory.Create( types );
				customizations.Apply( customization => fixture.Customize( customization ) );
			}
		}
	}
}