using AutoFixture;
using System;
using System.Linq;
using System.Threading;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class ManualPropertyTypesCustomization : CompositeCustomization
	{
		public static ManualPropertyTypesCustomization Default { get; } = new ManualPropertyTypesCustomization();

		ManualPropertyTypesCustomization() : this(typeof(Thread)) {}

		public ManualPropertyTypesCustomization(params Type[] types) :
			base(types.Select(x => new NoAutoPropertiesCustomization(x))) {}
	}
}