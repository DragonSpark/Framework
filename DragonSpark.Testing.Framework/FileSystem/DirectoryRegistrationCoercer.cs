using DragonSpark.Sources.Coercion;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class DirectoryRegistrationCoercer : DelegatedCoercer<string, FileSystemRegistration>
	{
		public static DirectoryRegistrationCoercer Default { get; } = new DirectoryRegistrationCoercer();
		DirectoryRegistrationCoercer() : base( FileSystemRegistration.Directory ) {}
	}
}