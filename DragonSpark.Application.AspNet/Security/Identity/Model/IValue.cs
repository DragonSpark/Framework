using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public interface IValue : ISelect<IValueProvider, object>;