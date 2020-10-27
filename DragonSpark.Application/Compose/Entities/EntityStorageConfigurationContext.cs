﻿using DragonSpark.Application.Entities;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection.Alterations;
using LightInject;
using Microsoft.AspNetCore.Identity;
using System;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class EntityStorageConfigurationContext<T, TUser>
		where T : Security.Identity.IdentityDbContext<TUser>
		where TUser : IdentityUser
	{
		readonly ApplicationProfileContext _context;
		readonly Action<IdentityOptions>   _configure;

		public EntityStorageConfigurationContext(ApplicationProfileContext context, Action<IdentityOptions> configure)
		{
			_context   = context;
			_configure = configure;
		}

		public ApplicationProfileContext SqlServer() => Configuration(SqlStorageConfiguration<T>.Default);

		public ApplicationProfileContext Configuration(Alter<StorageConfigurationBuilder> configuration)
			=> Configuration(configuration(new StorageConfigurationBuilder()).Get());

		public ApplicationProfileContext Configuration(IStorageConfiguration configuration)
			=> _context.Then(new ConfigureIdentityStorage<T, TUser>(configuration, _configure))
			           .Configure(Initialize.Default.Get);

		sealed class Initialize : IAlteration<BuildHostContext>
		{
			public static Initialize Default { get; } = new Initialize();

			Initialize() {}

			public BuildHostContext Get(BuildHostContext parameter)
				=> parameter.Decorate<T>((factory, context) => factory.GetInstance<IStorageInitializer<T>>()
				                                                      .Get(context));
		}
	}
}