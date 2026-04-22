using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Maui.Device.Notifications.Remote.Messages;

sealed class ComposeRegistrations : IResult<IConditional<string, IActionRegistration>>
{
    readonly Func<IEnumerable<IActionRegistration>> _registrations;

    public ComposeRegistrations(Func<IEnumerable<IActionRegistration>> registrations) => _registrations = registrations;

    public IConditional<string, IActionRegistration> Get() => _registrations().ToDictionary(x => x.Get()).ToTable();
}