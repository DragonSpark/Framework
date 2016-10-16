using DragonSpark.Commands;
using DragonSpark.Runtime.Assignments;
using DragonSpark.Sources;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Runtime.Assignments
{
	public class ValueAssignmentTests
	{
		const string Start = "Start";
		const string Finish = "Finish";

		[Fact]
		public void Assignment()
		{
			var source = new SuppliedSource<string>();
			using ( new ValueAssignment<string>( new Assign<string>( source ), new Value<string>( Start, Finish ) ).AsExecuted() )
			{
				Assert.Equal( Start, source.Get() );
			}
			Assert.Equal( Finish, source.Get() );
		}

		[Fact]
		public void Cover()
		{
			var dictionary = new Dictionary<int, bool>();
			using ( new Assignment<int, bool>( new DictionaryAssign<int, bool>( dictionary ), 10, true ).AsExecuted() )
			{
				Assert.True( dictionary.ContainsKey( 10 ) );
				Assert.Equal( dictionary[10], true );
			}
			Assert.False( dictionary.ContainsKey( 10 ) );
		}
	}
}