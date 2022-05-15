using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose.Deferred;

sealed class DeferredRegistrations : List<ICommand<IServiceCollection>> { }