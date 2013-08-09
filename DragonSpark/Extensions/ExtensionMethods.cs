using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Extensions
{
	public static class ExtensionMethods
	{
		/// <summary>
		/// Applies the action to each element in the list.
		/// </summary>
		/// <typeparam name="T">The enumerable item's type.</typeparam>
		/// <param name="enumerable">The elements to enumerate.</param>
		/// <param name="action">The action to apply to each item in the list.</param>
		public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action) {
			foreach(var item in enumerable)
				action(item);
		}

		/// <summary>
		/// Converts an expression into a <see cref="MemberInfo"/>.
		/// </summary>
		/// <param name="expression">The expression to convert.</param>
		/// <returns>The member info.</returns>
		public static MemberInfo GetMemberInfo(this System.Linq.Expressions.Expression expression) {
			var lambda = (LambdaExpression)expression;

			MemberExpression memberExpression;
			if(lambda.Body is UnaryExpression) {
				var unaryExpression = (UnaryExpression)lambda.Body;
				memberExpression = (MemberExpression)unaryExpression.Operand;
			}
			else
				memberExpression = (MemberExpression)lambda.Body;

			return memberExpression.Member;
		}
	}
}