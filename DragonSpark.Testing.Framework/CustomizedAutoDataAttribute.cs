using DragonSpark.Extensions;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Testing.Framework
{
	public abstract class CustomizedAutoDataAttribute : AutoDataAttribute
	{
		readonly Type[] cutomizationTypes;

		protected CustomizedAutoDataAttribute( params Type[] cutomizationTypes )
		{
			this.cutomizationTypes = cutomizationTypes;
		}

		Action<ICustomization> Customize => customization => Fixture.Customize( customization );

		public override IEnumerable<object[]> GetData( MethodInfo methodUnderTest )
		{
			methodUnderTest.DeclaringType.With( type =>
			{
				var items = type.Assembly.GetCustomAttributes()
					.Concat( type.GetCustomAttributes() )
					.Concat( methodUnderTest.GetCustomAttributes() )
					.OfType<ICustomization>()
					.Concat( new MethodContextCustomization( new MethodContext( methodUnderTest ) ).Append( cutomizationTypes.Where( typeof(ICustomization).CanActivate ).Select( Activator.CreateInstance<ICustomization> ) ) )
					.Prioritize().ToArray();
				items.Apply( Customize );
			} );

			var result = base.GetData( methodUnderTest );
			return result;
		}
	}

	public interface IMethodContext
	{
		MethodInfo MethodUnderTest { get; }
	}

	class MethodContext : IMethodContext
	{
		public MethodContext( MethodInfo info )
		{
			MethodUnderTest = info;
		}

		public MethodInfo MethodUnderTest { get; }
	}

	[Priority( Priority.AboveHigh )]
	public class MethodContextCustomization : ICustomization
	{
		readonly IMethodContext context;

		public MethodContextCustomization( IMethodContext context )
		{
			this.context = context;
		}

		public void Customize( IFixture fixture )
		{
			fixture.Inject( context );
		}
	}
}