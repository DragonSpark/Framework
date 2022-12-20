using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Objects.Entities.Generation;

public interface IPrincipalProperty : ISelect<Memory<PropertyInfo>, PropertyInfo?> {}