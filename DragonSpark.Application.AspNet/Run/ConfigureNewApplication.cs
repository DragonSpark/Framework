using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.AspNet.Run;

public record ConfigureNewApplication(Func<IHost, IApplication> New, ICommand<IApplication> Configure)
	: ConfigureNew<IHost, IApplication>(New, Configure);