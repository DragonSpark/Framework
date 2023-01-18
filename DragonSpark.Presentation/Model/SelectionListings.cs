using DragonSpark.Model.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Presentation.Model;

public static class SelectionListings
{
	public static ICollection<SelectionListing<T>> For<T>() where T : struct, Enum
		=> SelectionListings<T>.Default.Get();
}

public sealed class SelectionListings<T> : IResult<ICollection<SelectionListing<T>>> where T : struct, Enum
{
	public static SelectionListings<T> Default { get; } = new();

	SelectionListings() {}

	public ICollection<SelectionListing<T>> Get() => Enum.GetValues<T>()
	                                                     .Select(x => new SelectionListing<T>
	                                                     {
		                                                     Name = x.ToString(), Value = x
	                                                     })
	                                                     .ToArray();
}