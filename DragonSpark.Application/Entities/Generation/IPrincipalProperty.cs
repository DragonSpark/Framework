using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation;

public interface IPrincipalProperty : ISelect<Memory<PropertyInfo>, PropertyInfo?> {}