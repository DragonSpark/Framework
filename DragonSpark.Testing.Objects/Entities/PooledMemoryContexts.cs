﻿using DragonSpark.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DragonSpark.Testing.Objects.Entities
{
	public sealed class PooledMemoryContexts<T> : PooledContexts<T> where T : DbContext
	{
		public PooledMemoryContexts() : base(NewMemoryOptions<T>.Default.Get()) {}
	}

	public sealed class PooledSqlContexts<T> : PooledContexts<T> where T : DbContext
	{
		public PooledSqlContexts() : base(NewSqlOptions<T>.Default.Get()) {}
	}

	public class PooledContexts<T> : Contexts<T> where T : DbContext
	{
		public PooledContexts(DbContextOptions<T> options) : this(new PooledDbContextFactory<T>(options)) {}

		public PooledContexts(IDbContextFactory<T> factory) : base(factory) {}
	}
}