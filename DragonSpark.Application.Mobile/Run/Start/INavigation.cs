using System;
using System.Threading.Tasks;
using DragonSpark.Model.Selection;
using Uno.Extensions.Hosting;
using Uno.Extensions.Navigation;

namespace DragonSpark.Application.Mobile.Run.Start;

public interface INavigation : ISelect<IApplicationBuilder, Func<IServiceProvider, INavigator, Task>>;
