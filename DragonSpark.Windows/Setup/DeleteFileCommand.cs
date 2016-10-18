﻿using DragonSpark.Aspects.Exceptions;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Diagnostics.Exceptions;
using DragonSpark.Windows.FileSystem;
using System.IO;

namespace DragonSpark.Windows.Setup
{
	[ApplyAutoValidation, ApplySpecification( typeof(FileSystemInfoExistsSpecification) ), ApplyExceptionPolicy( typeof(SuppliedRetryPolicySource<IOException>) )]
	public sealed class DeleteFileCommand : CommandBase<IFileSystemInfo>
	{
		public static DeleteFileCommand Default { get; } = new DeleteFileCommand();
		DeleteFileCommand() {}

		public override void Execute( IFileSystemInfo parameter ) => parameter.Delete();
	}
}