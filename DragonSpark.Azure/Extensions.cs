﻿using DragonSpark.Azure.Queues;
using DragonSpark.Azure.Storage;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Azure;

public static class Extensions
{
	public static BuildHostContext WithAzureConfigurations(this BuildHostContext @this)
		=> Configure.Default.Get(@this);

	public static ISaveContent Save(this IContainer @this) => new SaveContent(@this.Write());

	public static IWrite Write(this IContainer @this) => new Write(@this.Get());
	public static IMove Move(this IContainer @this) => new Move(@this.Copy(), @this.Delete());
	public static ICopy Copy(this IContainer @this) => new Copy(@this.Get());
	public static IDelete Delete(this IContainer @this) => new Delete(@this.Get());

	public static IDeleteContents DeleteContents(this IContainer @this) => new DeleteContents(@this.Get());

	public static ISend Send(this IQueue @this, TimeSpan? life = null, TimeSpan? visibility = null)
		=> new Send(@this.Get(), life.GetValueOrDefault(DefaultLife.Default),
		            visibility.GetValueOrDefault(DefaultVisibility.Default));

	public static RegistrationResult Storage<T>(this IServiceCollection @this) where T : class, IContainer
		=> @this.Start<IContainer>()
		        .Forward<T>()
		        .Singleton()
		        .Then.Start<T>()
		        .Singleton();

	public static RegistrationResult Queue<T>(this IServiceCollection @this) where T : class, IQueue
		=> @this.Start<IQueue>()
		        .Forward<T>()
		        .Singleton()
		        .Then.Start<T>()
		        .Singleton();
}