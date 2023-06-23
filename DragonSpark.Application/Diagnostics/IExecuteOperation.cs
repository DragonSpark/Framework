using DragonSpark.Model.Operations.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics;

public interface IExecuteOperation : ISelecting<(Type Owner, ValueTask Operation), Exception?> {}