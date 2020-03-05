using DragonSpark.Model.Operations;
using System;

namespace DragonSpark.Presentation.Components {
	public interface IExceptions : IOperation<(Type Owner, Exception Exception)> {}
}