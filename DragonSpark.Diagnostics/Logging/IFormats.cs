using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Diagnostics.Logging
{
	public interface IFormats : ISelect<string, string>, IArray<string> {}
}