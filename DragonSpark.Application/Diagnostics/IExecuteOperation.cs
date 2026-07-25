using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Diagnostics;

public interface IExecuteOperation : ISelecting<(Type Owner, ValueTask Operation), Exception?>;