using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Testing.Framework
{
	public interface IMethodCustomization
	{
		void Customizing( MethodInfo methodUnderTest, Type[] parameterTypes );
		
		void Customized( MethodInfo methodUnderTest, Type[] parameterTypes, IEnumerable<object[]> parameters );
	}
}