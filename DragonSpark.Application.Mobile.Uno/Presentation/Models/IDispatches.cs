using System;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Uno.Presentation.Models;

public interface IDispatches : ISelect<Action, ICondition>;