using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Objects;

public interface IFormattedProjection<in T> : ISelect<string?, T, IProjection> {}