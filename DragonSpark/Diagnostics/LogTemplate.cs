namespace DragonSpark.Diagnostics
{
	public delegate void LogTemplate<in T>( string template, T parameter );
	public delegate void LogTemplate<in T1, in T2>( string template, T1 first, T2 second );
	public delegate void LogTemplate<in T1, in T2, in T3>( string template, T1 first, T2 second, T3 third );
	public delegate void LogTemplate( string template, params object[] parameters );
}