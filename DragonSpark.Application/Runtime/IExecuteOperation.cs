using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime
{
	public interface IExecuteOperation : ISelecting<(Type Owner, ValueTask Operation), Exception?> {}
}