using System.ComponentModel;

namespace DragonSpark
{
	public interface IPriorityAware
	{
		[DefaultValue( Priority.Normal )]
		Priority Priority { get; }
	}
}