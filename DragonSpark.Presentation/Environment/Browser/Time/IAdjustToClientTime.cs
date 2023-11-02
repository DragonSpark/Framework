using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Presentation.Environment.Browser.Time;

public interface IAdjustToClientTime : ISelect<DateTimeOffset, DateTimeOffset?>, ICondition;