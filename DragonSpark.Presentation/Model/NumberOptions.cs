using System.Linq;

namespace DragonSpark.Presentation.Model;

public class NumberOptions : Options<ushort>
{
	protected NumberOptions(params ushort[] options) : this("N0", options) {}

	protected NumberOptions(string format = "N0", params ushort[] options)
		: base(options.Select(x => new Option<ushort> { Name = x.ToString(format), Value = x })) {}
}
