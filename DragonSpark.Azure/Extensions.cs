using Azure.Core.Serialization;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Messaging.Messages.Queues.Durable;
using DragonSpark.Azure.Storage;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Azure;

public static class Extensions
{
	public static BuildHostContext WithAzureConfigurations(this BuildHostContext @this) => Configure.Default.Get(@this);

	public static ISaveContent Save(this IContainer @this) => new SaveContent(@this.Write());

	public static IPath Path(this IContainer @this) => new Path(@this.Get());

	public static IEntry Entry(this IContainer @this) => new Entry(@this.Get());
	
	public static ISnapshots Snapshots(this IContainer @this) => new Snapshots(new Snapshot(@this.Get()));

	public static ValueTask<ISnapshotEntry> Get(this ISnapshots @this, CancellationToken stop,
													  params ReadOnlySpan<string?> names)
	{
		using var promote = names.AsValueEnumerable()
								 .Where(x => x is not null)
								 .Select(x => x.Verify())
								 .ToArray(ArrayPool<string>.Shared);
		return Get(@this, new(promote, stop));
	}

	static async ValueTask<ISnapshotEntry> Get(this ISnapshots @this, Stop<Lease<string>> parameter)
	{
		var (subject, stop) = parameter;
		using (subject)
		{
			return await @this.Off(new(subject.Memory, stop));
		}
	}

	public static ISnapshot Snapshot(this IContainer @this) => new Snapshot(@this.Get());

	public static IWrite Write(this IContainer @this) => new PolicyAwareWrite(new Write(@this.Get()));

	public static IAppend Append(this IContainer @this) => new Append(@this.Get());

	public static IMove Move(this IContainer @this, IContainer destination)
		=> new Move(destination.Copy(), @this.Delete());

	public static IMove Move(this IContainer @this) => new Move(@this.Copy(), @this.Delete());

	public static ICopy Copy(this IContainer @this) => new Copy(@this.Get());

	public static IDelete Delete(this IContainer @this) => new Delete(@this.Get());

	public static IDeleteContents DeleteContents(this IContainer @this) => new DeleteContents(@this.Get());

	public static ValueTask<object?> ToObjectAsync(this BinaryData data, Type type,
												   CancellationToken cancellationToken = default)
		=> data.ToObjectAsync(type, JsonObjectSerializer.Default, cancellationToken);

	// ReSharper disable once TooManyArguments
	public static ValueTask<object?> ToObjectAsync(this BinaryData data, Type type, ObjectSerializer serializer,
												   CancellationToken cancellationToken = default)
		=> serializer.DeserializeAsync(data.ToStream(), type, cancellationToken);

	public static ISend Send(this ISender @this, TimeSpan? visibility = null, TimeSpan? life = null)
		=> @this.Get(new ScopedInput(visibility, life));

	public static RegistrationResult Storage<T>(this IServiceCollection @this) where T : class, IContainer
		=> @this.Start<IContainer>()
				.Forward<T>()
				.Singleton()
				.Then.Start<T>()
				.Singleton();

	public static IServiceCollection AddAzureKeyVaultSecret(this IServiceCollection @this)
		=> Data.AddAzureKeyVaultSecret.Default.Parameter(@this);

	/**/

	public static T Get<T>(this ISelect<IReadOnlyDictionary<string, object>, T> @this,
						   ProcessMessageEventArgs parameter) => @this.Get(parameter.Message);

	public static T Get<T>(this ISelect<IReadOnlyDictionary<string, object>, T> @this, ProcessEventArgs parameter)
	{
		var properties = parameter.Data.Properties;
		// ReSharper disable once SuspiciousTypeConversion.Global
		return @this.Get(properties as IReadOnlyDictionary<string, object> ?? properties.AsReadOnly());
	}

	public static T Get<T>(this ISelect<IReadOnlyDictionary<string, object>, T> @this,
						   ServiceBusReceivedMessage parameter)
		=> @this.Get(parameter.ApplicationProperties);

	/**/

	public static ModelBuilder WithDurableMessaging(this ModelBuilder parameter)
		=> EnableDurableMessaging.Default.Get(parameter);
}