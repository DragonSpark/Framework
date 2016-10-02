namespace DragonSpark.Runtime.Assignments
{
	public interface IAssign<in T1, in T2>
	{
		void Assign( T1 first, T2 second );
	}
}