using System;
using DragonSpark.Model.Selection;
using Foundation;

namespace DragonSpark.Application.Mobile.Uno.Platforms.iOS.Notifications;

sealed class ConvertToNSDateComponents : ISelect<DateTime, NSDateComponents>
{
    public static ConvertToNSDateComponents Default { get; } = new();

    ConvertToNSDateComponents() {}

    public NSDateComponents Get(DateTime parameter) => new()
    {
        Month  = parameter.Month,
        Day    = parameter.Day,
        Year   = parameter.Year,
        Hour   = parameter.Hour,
        Minute = parameter.Minute,
        Second = parameter.Second
    };
}