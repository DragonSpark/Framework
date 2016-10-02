using System;

namespace DragonSpark.Diagnostics
{
	public delegate void LogException<in T>( Exception exception, string template, T parameter );
}