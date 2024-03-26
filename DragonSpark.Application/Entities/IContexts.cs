using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities;

public interface IContexts : IResult<Scope>;

// TODO
public readonly record struct Scope(DbContext Owner, IDisposable Disposable)
{
	public Scope(DbContext Owner) : this(Owner, Owner) {}
}