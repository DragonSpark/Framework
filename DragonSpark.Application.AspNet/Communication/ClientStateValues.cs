using DragonSpark.Application.AspNet.Security;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.Primitives;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.AspNet.Communication;

sealed class ClientStateValues : IClientStateValues
{
	readonly ICurrentContext _context;

	public ClientStateValues(ICurrentContext context) => _context = context;

	public Array<string> Get() => _context.Get()
	                                      .Request.Cookies.AsValueEnumerable()
	                                      .Select(x => (string)new StringValues($"{x.Key}={x.Value}")!)
	                                      .ToArray();
}