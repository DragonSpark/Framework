using AutoFixture;
using AutoFixture.Kernel;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Application.Hosting.xUnit
{
	sealed class SelectCustomizations : Select<IFixture, IList<ISpecimenBuilder>>
	{
		public static SelectCustomizations Default { get; } = new SelectCustomizations();

		SelectCustomizations() : base(x => x.Customizations) {}
	}
}