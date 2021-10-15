using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

public interface IPropertyAssignmentDelegate : ISelect<PropertyInfo, Action<object, object>> {}