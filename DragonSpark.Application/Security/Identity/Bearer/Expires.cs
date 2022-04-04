using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Expires : ISelect<byte, DateTime>
{
	public static Expires Default { get; } = new();

	Expires() : this(Time.Default) {}

	readonly ITime _time;

	public Expires(ITime time) => _time = time;

	public DateTime Get(byte parameter) => _time.Get().UtcDateTime.AddDays(parameter);
}