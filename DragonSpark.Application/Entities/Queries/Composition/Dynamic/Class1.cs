namespace DragonSpark.Application.Entities.Queries.Composition.Dynamic
{
	class Class1 {}

/*    public IEnumerable<FilterDescriptor> Filters { get; set; }

	public IEnumerable<SortDescriptor> Sorts { get; set; }*/

	public class DynamicQueryParameters
	{
		public int? Skip { get; set; }

		public int? Top { get; set; }

		public string? OrderBy { get; set; }

		public string? Filter { get; set; }
	}
}