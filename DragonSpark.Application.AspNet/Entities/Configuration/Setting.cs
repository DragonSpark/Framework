using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

public sealed class Setting
{
	[MaxLength(32)]
	public string Id { get; init; } = null!;

	[MaxLength(256)]
	public string? Value { get; set; }
}