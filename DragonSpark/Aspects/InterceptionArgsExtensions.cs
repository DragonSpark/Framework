using PostSharp.Aspects;

namespace DragonSpark.Aspects
{
	public static class InterceptionArgsExtensions
	{
		public static object GetReturnValue( this MethodInterceptionArgs @this )
		{
			@this.Proceed();
			return @this.ReturnValue;
		}
		public static T GetReturnValue<T>( this MethodInterceptionArgs @this ) => (T)@this.GetReturnValue();
		
	}
}