using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public interface IMemberInfoLocator
	{
		MemberInfo Locate( MemberInfo info );
	}
}