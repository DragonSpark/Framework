using DragonSpark.Model.Operations;

namespace DragonSpark.Syncfusion.Queries
{
	public interface IQuery<T> : IAltering<Parameter<T>> {}
}