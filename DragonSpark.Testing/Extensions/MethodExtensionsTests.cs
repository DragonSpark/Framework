using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using System.Linq;
using System.Windows.Input;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class MethodExtensionsTests
	{
		[Fact]
		public void BasicFactory()
		{
			var method = new Func<object, object>( new Factory().To<IParameterizedSource<object, object>>().Get ).Method;
			var found = method.AccountForGenericDefinition();
			Assert.NotNull( found );
			Assert.NotSame( method, found );
			Assert.True( found.DeclaringType.IsGenericTypeDefinition );
		}

		[Fact]
		public void GenericFactory()
		{
			var method = new Func<object, bool>( new Factory().IsSatisfiedBy ).Method;
			var found = method.AccountForGenericDefinition();
			Assert.NotNull( found );
			Assert.NotSame( method, found );
			Assert.True( found.DeclaringType.IsGenericTypeDefinition );
		}

		[Fact]
		public void BasicCommand()
		{
			var method = new Action<object>( new Command().To<ICommand>().Execute ).Method;
			var found = method.AccountForGenericDefinition();
			Assert.NotNull( found );
			Assert.NotSame( method, found );
			Assert.True( found.DeclaringType.IsGenericTypeDefinition );
		}

		[Fact]
		public void GenericCommand()
		{
			var method = new Action<object>( new Command().Execute ).Method;
			var found = method.AccountForGenericDefinition();
			Assert.NotNull( found );
			Assert.Same( method, found );
			Assert.False( found.DeclaringType.IsGenericTypeDefinition );
		}

		[Fact]
		public void BreakingBuildTest()
		{
			var method = new Action<Action<string>>( new PurgeLoggerMessageHistoryCommand( new LoggerHistorySink().Self ).Execute ).Method;
			var found = method.AccountForGenericDefinition();
			Assert.NotNull( found );
			Assert.NotSame( method, found );
			Assert.True( found.DeclaringType.IsGenericTypeDefinition );
			var memberInfo = found.GetParameterTypes().Single();
			Assert.True( memberInfo.IsConstructedGenericType );
			Assert.True( memberInfo.ContainsGenericParameters );
			Assert.True( memberInfo.IsGenericType );
			Assert.False( memberInfo.IsGenericTypeDefinition );
			Assert.Equal( memberInfo.GetGenericTypeDefinition() , typeof(Action<>) );
		}

		class Factory : SpecificationParameterizedSource<object, object>
		{
			public Factory() : base( DragonSpark.Specifications.Common.Assigned, o => null ) {}
		}

		class Command : CommandBase<object>
		{
			public override void Execute( object parameter ) {}
		}
	}
}