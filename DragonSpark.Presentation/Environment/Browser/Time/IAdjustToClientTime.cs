using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Presentation.Environment.Browser.Time;

public interface IAdjustToClientTime : ISelect<DateTimeOffset, DateTimeOffset?>, ICondition;