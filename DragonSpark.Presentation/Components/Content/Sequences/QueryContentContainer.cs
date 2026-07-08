using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.Presentation.Components.Content.Sequences;

partial class QueryContentContainer<T>
{
	readonly Switch   _ready = false;
	
	[Parameter]
	public required bool EnableClientCover { get; set; } = true;
}