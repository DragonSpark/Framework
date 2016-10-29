using DragonSpark.Extensions;
using DragonSpark.TypeSystem;
using DragonSpark.TypeSystem.Metadata;
using JetBrains.Annotations;
using System;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.TypeSystem.Metadata
{
	public class MemberInfoDefinitionsTests
	{
		[Fact]
		public void Ambiguous()
		{
			var methodInfo = new Action<string>( new Source().Method ).Method;
			var located = (MethodInfo)MemberInfoDefinitions.Default.Get( methodInfo );
			Assert.NotEqual( methodInfo.GetParameterTypes().Only(), located.GetParameterTypes().Only());
		}

		[UsedImplicitly]
		class SourceMetadata
		{
			[UsedImplicitly]
			public void Method( int parameter ) {}
			[UsedImplicitly]
			public void Method( string parameter ) {}
		}

		class Source
		{
			public void Method( string parameter ) {}
		}
	}
}