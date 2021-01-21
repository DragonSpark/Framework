using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components.State
{
	public interface IUpdateActivity : IAssign<object, object>, ICommand<object> {}
}