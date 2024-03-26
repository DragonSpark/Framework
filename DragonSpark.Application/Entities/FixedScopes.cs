﻿using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Entities;

public sealed class FixedScopes : Instance<Scope>, IScopes
{
	public FixedScopes(DbContext context) : base(new (context, context)) {}
}