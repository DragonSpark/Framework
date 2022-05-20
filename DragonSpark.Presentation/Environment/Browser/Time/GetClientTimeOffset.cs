using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser.Time;

sealed class GetClientTimeOffset : ISelecting<IJSObjectReference, TimeSpan>
{
	public static GetClientTimeOffset Default { get; } = new();

	GetClientTimeOffset() : this(nameof(GetClientTimeOffset)) {}

	readonly string _name;

	public GetClientTimeOffset(string name) => _name = name;

	public async ValueTask<TimeSpan> Get(IJSObjectReference parameter)
		=> TimeSpan.FromMinutes(await parameter.InvokeAsync<ushort>(_name));
}