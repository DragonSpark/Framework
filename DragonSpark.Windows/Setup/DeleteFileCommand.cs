using DragonSpark.Aspects.Exceptions;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Diagnostics.Exceptions;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Setup
{
	[ApplyAutoValidation, ApplySpecification( typeof(FileSystemInfoExistsSpecification) ), ApplyExceptionPolicy( typeof(SuppliedRetryPolicySource<IOException>) )]
	public sealed class DeleteFileCommand : CommandBase<FileSystemInfoBase>
	{
		public static DeleteFileCommand Default { get; } = new DeleteFileCommand();
		DeleteFileCommand() {}

		public override void Execute( FileSystemInfoBase parameter ) => parameter.Delete();
	}
}