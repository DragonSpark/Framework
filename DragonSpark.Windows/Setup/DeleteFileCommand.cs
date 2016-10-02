using DragonSpark.Aspects.Exceptions;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Diagnostics.Exceptions;
using System.IO;

namespace DragonSpark.Windows.Setup
{
	[ApplyAutoValidation, ApplySpecification( typeof(FileSystemInfoExistsSpecification) ), ApplyExceptionPolicy( typeof(SuppliedRetryPolicySource<IOException>) )]
	public sealed class DeleteFileCommand : CommandBase<FileSystemInfo>
	{
		public static DeleteFileCommand Default { get; } = new DeleteFileCommand();
		DeleteFileCommand() {}

		public override void Execute( FileSystemInfo parameter )
		{
			parameter.Delete();

		}
	}
}