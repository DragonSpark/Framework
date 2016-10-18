namespace DragonSpark.Testing.Framework.FileSystem
{
	public interface IPathingValidator
	{
		void IsLegalAbsoluteOrRelative( string pathToValidate, string paramName );
	}
}