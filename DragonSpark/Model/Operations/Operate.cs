using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public delegate ValueTask<TOut> Operate<TIn, TOut>(TIn parameter);
}