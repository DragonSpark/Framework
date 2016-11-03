using DragonSpark.Sources.Coercion;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class FileRegistrationCoercer : DelegatedCoercer<string, FileSystemRegistration>
	{
		public static FileRegistrationCoercer Default { get; } = new FileRegistrationCoercer();
		FileRegistrationCoercer() : base( FileSystemRegistration.File ) {}
	}
}