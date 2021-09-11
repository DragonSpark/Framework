﻿using DragonSpark.Application.Security.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class IdentityStorageType<T, TContext> where TContext : DbContext where T : IdentityUser
	{
		readonly ApplicationProfileContext _subject;
		readonly Action<IdentityOptions>   _configure;

		public IdentityStorageType(ApplicationProfileContext subject, Action<IdentityOptions> configure)
		{
			_subject   = subject;
			_configure = configure;
		}

		public IdentityStorageUsing<T, TContext> Application()
			=> new(_subject.Then(Registrations<TContext, T>.Default)
			               .Then(AddIdentityComponents<T>.Default)
			               .Then(DragonSpark.Application.Entities.Registrations<TContext>.Default),
			       _configure);

		public IdentityStorageUsing<T, TContext> Is
			=> new(_subject.Then(DragonSpark.Application.Entities.Registrations<TContext>.Default), _configure);
	}
}