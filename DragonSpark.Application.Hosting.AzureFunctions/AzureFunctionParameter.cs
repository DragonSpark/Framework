using System.Collections.Immutable;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Hosting.AzureFunctions
{
	public readonly struct AzureFunctionParameter
	{
		public AzureFunctionParameter(HttpRequest request, TraceWriter writer) : this(request, writer,
		                                                                              Empty<object>.Array) {}

		public AzureFunctionParameter(HttpRequest request, TraceWriter writer, params object[] services)
			: this(request, writer, services.ToImmutableArray()) {}

		public AzureFunctionParameter(HttpRequest request, TraceWriter writer, ImmutableArray<object> services)
		{
			Request  = request;
			Writer   = writer;
			Services = services;
		}

		public HttpRequest Request { get; }

		public TraceWriter Writer { get; }

		public ImmutableArray<object> Services { get; }
	}
}