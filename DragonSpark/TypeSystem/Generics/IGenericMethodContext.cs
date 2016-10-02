using System;

namespace DragonSpark.TypeSystem.Generics
{
	public interface IGenericMethodContext<T> where T : class
	{
		MethodContext<T> Make( params Type[] types );
	}
}