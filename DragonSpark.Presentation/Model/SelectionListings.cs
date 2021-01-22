using DragonSpark.Application;
using DragonSpark.Application.Runtime;
using DragonSpark.Model.Results;
using System;
using System.Linq;

namespace DragonSpark.Presentation.Model
{
	public static class SelectionListings
	{
		public static SelectedCollection<SelectionListing<T>> For<T>() where T : struct, Enum
			=> SelectionListings<T>.Default.Get();
	}

	public sealed class SelectionListings<T> : IResult<SelectedCollection<SelectionListing<T>>> where T : struct, Enum
	{
		public static SelectionListings<T> Default { get; } = new SelectionListings<T>();

		SelectionListings() {}

		public SelectedCollection<SelectionListing<T>> Get() => Enum.GetValues<T>()
		                                                            .Select(x => new SelectionListing<T>
		                                                            {
			                                                            Name = x.ToString(), Value = x
		                                                            })
		                                                            .ToSelectedCollection();
	}
}