using System.ComponentModel;

namespace DragonSpark
{
	public interface IAllowsPriority
	{
		[DefaultValue( Priority.Normal )]
		Priority Priority { get; }
	}
}