using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Hosting.Uno.Run;

public interface IConfigureApplicationBuilder : ICommand<IApplicationBuilder>, ICommand<Mobile.Uno.Run.Application> {}