using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Text;
using Microsoft.JSInterop;
using System;

namespace DragonSpark.Presentation.Environment.Browser;

public class LoadModule : Resulting<IJSObjectReference>
{
	protected LoadModule(ModuleReference load, Type reference)
		: this(load, ModuleResourcePath.Default.Get(reference)) {}

	protected LoadModule(ModuleReference load, string path) : base(load.Then().Bind(path)) {}
}

public class LoadModule<T> : LoadModule
{
	public LoadModule(ModuleReference load) : base(load, A.Type<T>()) {}
}

// TODO:

sealed class ModuleResourcePath : IFormatter<Type>
{
	public static ModuleResourcePath Default { get; } = new();

	ModuleResourcePath() {}

	public string Get(Type parameter)
	{
		var assembly   = parameter.Assembly.GetName().Name.Verify();
		var @namespace = parameter.Namespace.Verify().Replace(assembly, string.Empty).Replace(".", "/");
		var result     = $"./_content/{assembly}/{@namespace}/{parameter.Name}.js";
		return result;
	}
}