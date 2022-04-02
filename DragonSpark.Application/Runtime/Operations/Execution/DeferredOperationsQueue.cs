using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class DeferredOperationsQueue : Queue<Func<ValueTask>> {}