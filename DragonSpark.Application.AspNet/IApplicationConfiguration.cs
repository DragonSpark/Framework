using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.AspNet;

public interface IApplicationConfiguration : ICommand<IApplicationBuilder>;