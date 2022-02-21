using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.JSInterop;
using System;

namespace DragonSpark.Presentation.Environment.Browser;

public class LoadModule : Resulting<IJSObjectReference>
{
	protected LoadModule(ModuleReference load, Type reference)
		: this(load, $"./_content/{reference.Assembly.GetName().Name}/{reference.Name}.js") {}

	protected LoadModule(ModuleReference load, string path) : base(load.Then().Bind(path)) {}
}

public class LoadModule<T> : LoadModule
{
	public LoadModule(ModuleReference load) : base(load, A.Type<T>()) {}
}