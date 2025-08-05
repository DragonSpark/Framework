using System.Collections.Generic;

namespace DragonSpark.Application.Mobile.Model.Validation;

public readonly record struct ValidationModelRecord(Dictionary<string, string[]> Local, Dictionary<string, string[]> External);