using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Diagnostics
{
	public interface IExecuteOperation : ISelecting<(Type Owner, ValueTask Operation), Exception?> {}
}