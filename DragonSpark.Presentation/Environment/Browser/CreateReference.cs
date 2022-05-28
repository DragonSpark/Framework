﻿using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public class CreateReference<T> : ISelecting<CreateReferenceInput<T>, IJSObjectReference> where T : IArray<object>
{

	readonly string _name;

	public CreateReference(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(CreateReferenceInput<T> parameter)
	{
		var (reference, input) = parameter;
		return reference.InvokeAsync<IJSObjectReference>(_name, input.Get().Open());
	}
}

public class CreateReference : IAltering<IJSObjectReference>
{
	readonly string _name;

	public CreateReference(string name) => _name = name;

	public ValueTask<IJSObjectReference> Get(IJSObjectReference parameter)
		=> parameter.InvokeAsync<IJSObjectReference>(_name);
}