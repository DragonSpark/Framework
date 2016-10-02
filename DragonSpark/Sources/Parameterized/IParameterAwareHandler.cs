namespace DragonSpark.Sources.Parameterized
{
	public interface IParameterAwareHandler
	{
		bool Handles( object parameter );

		bool Handle( object parameter, out object handled );
	}
}