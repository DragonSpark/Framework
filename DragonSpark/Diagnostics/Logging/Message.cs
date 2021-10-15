namespace DragonSpark.Diagnostics.Logging;

public delegate void Message<in T>(string template, T parameter);

public delegate void Message<in T1, in T2>(string template, T1 first, T2 second);

public delegate void Message<in T1, in T2, in T3>(string template, T1 first, T2 second, T3 third);

public delegate void Message(string template, params object[] parameters);