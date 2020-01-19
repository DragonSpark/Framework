using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Performance", "EPS06:Hidden struct copy operation", Justification           = "<Pending>")]
[assembly: SuppressMessage("Bug", "ERP001:Non-observed return value of the pure method.", Justification = "<Pending>")]

[assembly:
	SuppressMessage("Performance", "EPS06:Hidden struct copy operation", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>", Scope = "member", Target = "~M:DragonSpark.Testing.Application.ValueIterationBenchmarks.Call(System.Int32)")]