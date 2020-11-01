using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.Components.Validation
{
	public interface IValidatorKey<T> : ISelect<ValidationContext, T?>, IAssign<ValidationContext, T> where T : class {}
}