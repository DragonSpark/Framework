using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Objects;

public interface IProjectors : ISelect<Type, string, Func<object, IProjection>> {}