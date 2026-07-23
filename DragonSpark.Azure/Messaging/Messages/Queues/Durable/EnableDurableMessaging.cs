using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class EnableDurableMessaging : IAlteration<ModelBuilder>
{
	public static EnableDurableMessaging Default { get; } = new();

	EnableDurableMessaging() {}

	public ModelBuilder Get(ModelBuilder parameter)
		=> parameter.Entity<ProcessNotification>(x =>
		                                         {
			                                         x.HasIndex(y => new { y.AvailableAt, y.Sent })
			                                          .HasFilter("[Sent] IS NULL");
			                                         x.HasOne(y => y.Subject)
			                                          .WithOne()
			                                          .HasPrincipalKey<ExternalProcess>();
		                                         });
}