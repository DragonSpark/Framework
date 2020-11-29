using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Xunit.Abstractions;

namespace DragonSpark.Application.Hosting.xUnit
{
	// ReSharper disable LocalSuppression

	sealed class Decorated : IMethodInfo
	{
		static Exception Unwrap(Exception ex)
		{
			while (true)
			{
				if (!(ex is TargetInvocationException exception))
				{
					return ex;
				}

				ex = exception.InnerException!;
			}
		}

		readonly IMethodInfo _method;

		public Decorated(IMethodInfo method) => _method = method;

		public IEnumerable<IAttributeInfo> GetCustomAttributes(string assemblyQualifiedAttributeTypeName)
		{
			try
			{
				return _method.GetCustomAttributes(assemblyQualifiedAttributeTypeName);
			}
			catch (Exception e)
			{
				// ReSharper disable once UnthrowableException
				// ReSharper disable once ThrowingSystemException
				throw Unwrap(e).Demystify();
			}
		}

		public IEnumerable<ITypeInfo> GetGenericArguments() => _method.GetGenericArguments();

		public IEnumerable<IParameterInfo> GetParameters() => _method.GetParameters();

		public IMethodInfo MakeGenericMethod(params ITypeInfo[] typeArguments)
			=> _method.MakeGenericMethod(typeArguments);

		public bool IsAbstract => _method.IsAbstract;

		public bool IsGenericMethodDefinition => _method.IsGenericMethodDefinition;

		public bool IsPublic => _method.IsPublic;

		public bool IsStatic => _method.IsStatic;

		public string Name => _method.Name;

		public ITypeInfo ReturnType => _method.ReturnType;

		public ITypeInfo Type => _method.Type;
	}
}