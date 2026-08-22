using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Components;

namespace DragonSpark.SyncfusionRendering.Queries;

public readonly record struct DataRequestsInput(
	ComponentBase Owner,
	string Identity,
	Switch Active,
	IDataRequest Request);