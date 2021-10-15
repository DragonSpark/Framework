namespace DragonSpark.Diagnostics.Logging;

public delegate void Exception<in T>(System.Exception exception, string template, T parameter);

public delegate void Exception<in T1, in T2>(System.Exception exception, string template, T1 first, T2 second);

public delegate void Exception<in T1, in T2, in T3>(System.Exception exception, string template, T1 first,
                                                    T2 second,
                                                    T3 third);

public delegate void Exception(System.Exception exception, string template, params object[] parameters);