using System;
using System.Threading.Tasks;
using DragonSpark.Model.Selection;
using Uno.Extensions.Hosting;
using Uno.Extensions.Navigation;

namespace DragonSpark.Application.Mobile.Uno.Run.Start;

public interface INavigationStart : ISelect<IApplicationBuilder, Func<IServiceProvider, INavigator, Task>>;
