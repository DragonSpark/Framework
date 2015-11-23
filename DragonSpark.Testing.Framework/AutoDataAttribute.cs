using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	/*public class RegisterPoliciesCustomization : ICustomization
	{
		public void Customize( IFixture fixture )
		{
			var method = fixture.GetCurrentMethod().GetParameters().Select( info => info.ParameterType );
		}
	}*/

	public class AutoDataAttribute : Ploeh.AutoFixture.Xunit2.AutoDataAttribute
	{
		readonly Type[] customizationTypes;

		public AutoDataAttribute() : this( typeof(Customizations.Default) )
		{}

		public AutoDataAttribute( params Type[] customizationTypes ) : this( new Fixture( GreedyEngineParts.Instance ), customizationTypes )
		{}

		public AutoDataAttribute( IFixture fixture, Type[] customizationTypes ) : base( fixture )
		{
			this.customizationTypes = customizationTypes;
		}
		
		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			new CurrentTestCustomizer( methodUnderTest, customizationTypes ).Customize( Fixture );
			
			var result = base.GetData( methodUnderTest );
			return result;
		}
	}
}