using AutoFixture.Kernel;
using System.Reflection;

namespace DragonSpark.Application.Hosting.xUnit
{
	public class ParameterRequestRelay : ISpecimenBuilder
	{
		readonly static NoSpecimen NoSpecimen = new NoSpecimen();

		readonly AutoFixture.Kernel.ParameterRequestRelay _inner;

		public ParameterRequestRelay(AutoFixture.Kernel.ParameterRequestRelay inner) => _inner = inner;

		public object? Create(object request, ISpecimenContext context)
			=> request is ParameterInfo parameter
				   ? parameter.IsOptional ? parameter.DefaultValue : _inner.Create(request, context)
				   : NoSpecimen;
	}
}