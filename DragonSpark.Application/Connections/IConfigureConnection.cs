using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Http.Connections.Client;

namespace DragonSpark.Application.Connections;

public interface IConfigureConnection : ICommand<HttpConnectionOptions> {}