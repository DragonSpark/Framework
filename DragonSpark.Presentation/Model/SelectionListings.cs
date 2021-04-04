using DragonSpark.Model.Results;
using System;
using System.Linq;

namespace DragonSpark.Presentation.Model
{
	public static class SelectionListings
	{
		public static SelectionListingCollection<T> For<T>() where T : struct, Enum
			=> SelectionListings<T>.Default.Get();
	}

	public sealed class SelectionListings<T> : IResult<SelectionListingCollection<T>> where T : struct, Enum
	{
		public static SelectionListings<T> Default { get; } = new SelectionListings<T>();

		SelectionListings() {}

		public SelectionListingCollection<T> Get() => new(Enum.GetValues<T>()
		                                                      .Select(x => new SelectionListing<T>
		                                                      {
			                                                      Name = x.ToString(), Value = x
		                                                      }));
	}
}