using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

public record PageRequest<T>(
	T Subject,
	bool Count = true,
	byte? Top = null,
	uint? Skip = null,
	string? Filter = null,
	string? OrderBy = null)
	: PageRequest(Count, Top, Skip, Filter, OrderBy);

[ModelBinder(BinderType = typeof(PageRequestBinder))]
public record PageRequest(
	bool Count = true,
	byte? Top = null,
	uint? Skip = null,
	string? Filter = null,
	string? OrderBy = null)
{
	public PageRequest<T> Input<T>(T parameter) => new(parameter, Count, Top, Skip, Filter, OrderBy);
}