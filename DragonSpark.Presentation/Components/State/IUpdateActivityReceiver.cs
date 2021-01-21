using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.State
{
	public interface IUpdateActivityReceiver : IAssigning<object, object>, IOperation<object> {}
}