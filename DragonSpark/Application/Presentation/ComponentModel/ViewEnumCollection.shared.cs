using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
	public class ViewEnumCollection<T> : ViewCollection<BindableEnumerationItem> where T : struct
	{
		public ViewEnumCollection(params T[] values)
			: this()
		{
			var toRemove = from bindableEnum in this
			               where !values.Contains((T)bindableEnum.Value)
			               select bindableEnum;

			toRemove.ToList().Apply(x => Remove(x));
		}

		public ViewEnumCollection()
		{
			var type = typeof(T);

			if(!type.IsEnum)
				throw new ArgumentException("This class only supports Enum types.");

			var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);

			foreach(var field in fields)
			{
				var att = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
					.OfType<DescriptionAttribute>().FirstOrDefault();

				var bindableEnum = new BindableEnumerationItem
				                   	{
				                   		Value = field.GetValue(null),
				                   		UnderlyingValue = (int)field.GetValue(null),
				                   		DisplayName = att != null ? att.Description : field.Name
				                   	};

				Add(bindableEnum);
			}
		}
	}
}