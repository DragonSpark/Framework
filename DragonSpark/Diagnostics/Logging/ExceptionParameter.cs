namespace DragonSpark.Diagnostics.Logging;

public readonly record struct ExceptionParameter<T>(System.Exception Exception, T Argument);