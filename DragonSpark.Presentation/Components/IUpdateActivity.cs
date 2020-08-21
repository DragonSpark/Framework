using DragonSpark.Model.Commands;

namespace DragonSpark.Presentation.Components
{
	public interface IUpdateActivity : ICommand<object>, IAssign<object, object> {}
}