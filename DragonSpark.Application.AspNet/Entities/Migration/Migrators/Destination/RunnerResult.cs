using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public readonly record struct RunnerResult(Task Work, ChannelReader<DbContext> Reader);