using AutoFixture;
using AutoFixture.AutoMoq;
using DragonSpark.Model;
using System.Linq;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class DefaultCustomizations : CompositeCustomization
	{
		public static DefaultCustomizations Default { get; } = new DefaultCustomizations();

		DefaultCustomizations() : this(Empty.Array<ICustomization>()) {}


		public DefaultCustomizations(params ICustomization[] others)
			: base(new ICustomization[]
			{
				ManualPropertyTypesCustomization.Default,
				SingletonCustomization.Default,
				new InsertCustomization(TimeSpecimen.Default),
				new InsertCustomization(EpochSpecimen.Default),
				new AutoMoqCustomization { ConfigureMembers = true }
			}.Concat(others)) {}
	}
}