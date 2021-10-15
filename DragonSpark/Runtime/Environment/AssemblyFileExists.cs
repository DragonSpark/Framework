using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Activation;
using System.Reflection;

namespace DragonSpark.Runtime.Environment;

sealed class AssemblyFileExists : Condition<Assembly>, IActivateUsing<IAlteration<string>>
{
	public AssemblyFileExists(IAlteration<string> alter)
		: base(AssemblyLocation.Default.Select(LocalFilePath.Default)
		                       .Select(alter)
		                       .Select(FilePathExists.Default)
		                       .Then()) {}
}