using System;
using System.Reflection;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Ploeh.AutoFixture.Xunit;

namespace DragonSpark.Testing.Framework
{
	[AttributeUsage( AttributeTargets.Parameter, AllowMultiple = false )]
	public class FreezeObjectAttribute : CustomizeAttribute
	{
		public override ICustomization GetCustomization( ParameterInfo parameter )
		{
			var result = new CompositeCustomization( new FreezingCustomization( As ?? parameter.ParameterType, parameter.ParameterType ), new Customization( parameter.ParameterType ) );
			return result;
		}

		public Type As { get; set; }

		class Customization : ICustomization
		{
			readonly Type targetType;

			public Customization( Type targetType )
			{
				this.targetType = targetType;
			}

			public void Customize( IFixture fixture )
			{
				var specimen = new SpecimenContext( fixture ).Resolve( new SeededRequest( targetType, null ) );
				
				var item = specimen.AsTo<Mock, object>( x => x.Object );
				var type = targetType.GetItemType();

				var locator = fixture.Create<IServiceLocator>();
				locator.Register( type, item );
			}
		}
	}
}