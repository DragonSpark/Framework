namespace DragonSpark.Runtime.Assignments
{
	public interface IAssign<in T>
	{
		void Assign( T first );
	}
}
