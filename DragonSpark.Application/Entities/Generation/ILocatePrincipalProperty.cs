using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation;

public interface ILocatePrincipalProperty : ISelect<Type, PropertyInfo?> {}