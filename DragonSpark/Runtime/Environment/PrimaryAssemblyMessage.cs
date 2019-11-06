using System.Reflection;
using DragonSpark.Text;

namespace DragonSpark.Runtime.Environment
{
	sealed class PrimaryAssemblyMessage : Message<Assembly>
	{
		public static PrimaryAssemblyMessage Default { get; } = new PrimaryAssemblyMessage();

		PrimaryAssemblyMessage() :
			base(_ => "A request was made for this application's primary assembly, but one could not be located.  Please ensure the entry or primary assembly or executable used for this application is marked with the PrimaryAssemblyAttribute.") {}
	}
}