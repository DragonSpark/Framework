using System;

namespace DragonSpark.Diagnostics
{
	public delegate void LogException<in T>( Exception exception, string template, T parameter );
	public delegate void LogException<in T1, in T2>( Exception exception, string template, T1 first, T2 second );
	public delegate void LogException<in T1, in T2, in T3>( Exception exception, string template, T1 first, T2 second, T3 third );
	public delegate void LogException( Exception exception, string template, params object[] parameters );
}