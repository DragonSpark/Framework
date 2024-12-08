﻿using DragonSpark.Application.AspNet.Entities.Configure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace DragonSpark.Application.AspNet.Compose.Entities;

public sealed class IdentityStorageConfiguration<T, TContext> where TContext : DbContext where T : class
{
	readonly ApplicationProfileContext _subject;
	readonly Action<IdentityOptions>   _configure;

	public IdentityStorageConfiguration(ApplicationProfileContext subject, Action<IdentityOptions> configure)
	{
		_subject   = subject;
		_configure = configure;
	}

	public ConfiguredIdentityStorage<T, TContext> SqlServer()
		=> Configuration(SqlStorageConfiguration<TContext>.Default);

	public ConfiguredIdentityStorage<T, TContext> SqlServer(Action<SqlServerDbContextOptionsBuilder> configuration)
		=> Configuration(new SqlStorageConfiguration<TContext>(configuration));

	public ConfiguredIdentityStorage<T, TContext> Configuration(IStorageConfiguration configuration)
		=> new(_subject, _configure, configuration);
}