using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public readonly record struct EachInput<T>(
	DbContext Origin,
	IResult<Workspace> Workspace,
	Array<T> Page,
	ChannelWriter<DbContext> Writer);