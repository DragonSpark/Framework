using Azure.Core.Serialization;
using Azure.Messaging.EventHubs.Processor;
using Azure.Messaging.ServiceBus;
using DragonSpark.Azure.Data;
using DragonSpark.Azure.Messaging.Messages.Queues.Durable;
using DragonSpark.Azure.Configuration;
using DragonSpark.Azure.Storage;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Azure;

public static class Extensions
{
	extension(BuildHostContext @this)
	{
		public BuildHostContext WithAzureConfigurations() => Configure.Default.Get(@this);

		public BuildHostContext WithBackgroundSweeper()
			=> @this.Configure(Messaging.Messages.Queues.Durable.Registrations.Default);

		public BuildHostContext WithUploadSupport() => @this.Configure(Azure.Storage.Uploads.Registrations.Default);
	}

	extension(IContainer @this)
	{
		public ISaveContent Save() => new SaveContent(@this.Write());

		public IPath Path() => new Storage.Path(@this.Get());

		public IEntry Entry() => new Entry(@this.Get());

		public ISnapshots Snapshots() => new Snapshots(new Snapshot(@this.Get()));
	}

	extension(ISnapshots @this)
	{
		public ValueTask<ISnapshotEntry> Get(CancellationToken stop,
		                                     params ReadOnlySpan<string?> names)
		{
			using var promote = names.AsValueEnumerable()
			                         .Where(x => x is not null)
			                         .Select(x => x.Verify())
			                         .ToArray(ArrayPool<string>.Shared);
			return Get(@this, new(promote, stop));
		}

		async ValueTask<ISnapshotEntry> Get(Stop<Lease<string>> parameter)
		{
			var (subject, stop) = parameter;
			using (subject)
			{
				return await @this.Off(new(subject.Memory, stop));
			}
		}
	}

	extension(IContainer @this)
	{
		public ISnapshot Snapshot() => new Snapshot(@this.Get());

		public IWrite Write() => new PolicyAwareWrite(new Write(@this.Get()));

		public IAppend Append() => new Append(@this.Get());

		public IMove Move(IContainer destination)
			=> new Move(destination.Copy(), @this.Delete());

		public IMove Move() => new Move(@this.Copy(), @this.Delete());

		public ICopy Copy() => new Copy(@this.Get());

		public IDelete Delete() => new Delete(@this.Get());

		public IDeleteContents DeleteContents() => new DeleteContents(@this.Get());
	}

	extension(BinaryData data)
	{
		public ValueTask<object?> ToObjectAsync(Type type,
		                                        CancellationToken cancellationToken = default)
			=> data.ToObjectAsync(type, JsonObjectSerializer.Default, cancellationToken);

		public ValueTask<object?> ToObjectAsync(Type type, ObjectSerializer serializer,
		                                        CancellationToken cancellationToken = default)
			=> serializer.DeserializeAsync(data.ToStream(), type, cancellationToken);
	}

	// ReSharper disable once TooManyArguments

	public static ISend Send(this ISender @this, TimeSpan? visibility = null, TimeSpan? life = null)
		=> @this.Get(new ScopedInput(visibility, life));

	extension(IServiceCollection @this)
	{
		public RegistrationResult Storage<T>() where T : class, IContainer
			=> @this.Start<IContainer>()
			        .Forward<T>()
			        .Singleton()
			        .Then.Start<T>()
			        .Singleton();

		public IServiceCollection AddAzureKeyVaultSecret()
			=> Data.AddAzureKeyVaultSecret.Default.Parameter(@this);
	}

	extension(IDataProtectionBuilder @this)
	{
		public IDataProtectionBuilder Hosted() => HostedKeys.Default.Get(@this);
	}
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

	public static IServiceCollection WithEnvironmentalCredential(this IServiceCollection @this)
		=> EnvironmentAwareConfiguration.Default.Parameter(@this);
}