using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Invocation;
using System.IO;

namespace DragonSpark.Runtime.Environment
{
	class RuntimeFile : Invocation0<string, string, string>, IAlteration<string>
	{
		protected RuntimeFile(string extension) : base(Path.ChangeExtension, extension) {}
	}
}