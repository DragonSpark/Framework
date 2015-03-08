using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Extensions
{
	/// <summary>
	/// Various extension methods used by the framework.
	/// </summary>
	public static class CoreExtensions
	{
		/*/// <summary>
		/// Enables customization of the discoverable types within a given assembly.
		/// </summary>
		public static Func<Assembly, IEnumerable<Type>> GetInspectableTypes = assembly => assembly.GetExportedTypes();*/

		/*/// <summary>
		/// Configures the core.
		/// </summary>
		/// <param name="hook">The hook.</param>
		/// <returns></returns>
		public static CoreConfiguration Core(this IModuleHook hook)
		{
			return CaliburnModule<CoreConfiguration>.Instance;
		}*/

		/// <summary>
		/// Gets the first or default of the given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <returns></returns>
		public static T FirstOrDefaultOfType<T>(this IEnumerable enumerable)
		{
			return enumerable.OfType<T>().FirstOrDefault();
		}

		/*/// <summary>
		/// Gets the attributes.
		/// </summary>
		/// <typeparam name="T">The attribute type.</typeparam>
		/// <param name="member">The member.</param>
		/// <param name="inherit">if set to <c>true</c> [inherit].</param>
		/// <returns>The attributes.</returns>
		public static IEnumerable<T> GetAttributes<T>(this MemberInfo member, bool inherit)
		{
			return Attribute.GetCustomAttributes(member, inherit).OfType<T>();
		}

		/// <summary>
		/// Applies the specified action to each item in the collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="action">The action.</param>
		public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
		{
			foreach(var item in enumerable)
			{
				action(item);
			}
		}
		*/
		/*/// <summary>
		/// Gets the member info represented by an expression.
		/// </summary>
		/// <param name="expression">The member expression.</param>
		/// <returns>The member info represeted by the expression.</returns>
		public static MemberInfo GetMemberInfo(this System.Linq.Expressions.Expression expression)
		{
			var lambda = (LambdaExpression)expression;

			MemberExpression memberExpression;
			if (lambda.Body is UnaryExpression)
			{
				var unaryExpression = (UnaryExpression)lambda.Body;
				memberExpression = (MemberExpression)unaryExpression.Operand;
			}
			else memberExpression = (MemberExpression)lambda.Body;

			return memberExpression.Member;
		}*/

		/// <summary>
		/// Gets a property by name, ignoring case and searching all interfaces.
		/// </summary>
		/// <param name="type">The type to inspect.</param>
		/// <param name="propertyName">The property to search for.</param>
		/// <returns>The property or null if not found.</returns>
		public static PropertyInfo GetPropertyCaseInsensitive(this Type type, string propertyName)
		{
			var typeList = new List<Type> { type };

			if (type.IsInterface)
				typeList.AddRange(type.GetInterfaces());

			return typeList
				.Select(interfaceType => interfaceType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance))
				.FirstOrDefault(property => property != null);
		}
	}
}