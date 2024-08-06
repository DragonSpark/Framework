using DragonSpark.Model.Operations;

namespace DragonSpark.Presentation.Components.State;

public interface IUpdateActivityReceiver : IAssigning<object, ActivityReceiver>, IOperation<object>;