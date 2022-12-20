using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Objects.Entities.Generation;

public interface ILocatePrincipalProperty : ISelect<Type, PropertyInfo?> {}