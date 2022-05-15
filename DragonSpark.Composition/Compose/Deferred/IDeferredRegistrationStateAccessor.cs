using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose.Deferred;

interface IDeferredRegistrationStateAccessor : ISelect<IDictionary<object, object>, DeferredRegistrations>,
                                               IAssign<IDictionary<object, object>, DeferredRegistrations>
{ }