using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

public readonly record struct WriterInput<T>(
	DbContext Source, // TODO
	IResult<Workspace> Workspaces,
	Array<T> Items,
	ChannelWriter<DbContext> Writer);