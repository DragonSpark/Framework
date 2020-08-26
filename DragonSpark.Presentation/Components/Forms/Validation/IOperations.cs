using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public interface IOperations : ICommand<Task>, ICommand, ICondition, IOperation {}
}