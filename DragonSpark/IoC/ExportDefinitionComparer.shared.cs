using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using DragonSpark.Extensions;

namespace DragonSpark.IoC
{
	public class ExportDefinitionComparer : IEqualityComparer<ExportDefinition>
	{
		public static ExportDefinitionComparer Instance
		{
			get { return InstanceField; }
		}	static readonly ExportDefinitionComparer InstanceField = new ExportDefinitionComparer();

		bool IEqualityComparer<ExportDefinition>.Equals( ExportDefinition x, ExportDefinition y )
		{
			var result = x.To<ICompositionElement>().Origin.DisplayName == y.To<ICompositionElement>().Origin.DisplayName;
			return result;
		}

		int IEqualityComparer<ExportDefinition>.GetHashCode( ExportDefinition obj )
		{
			var result = obj.To<ICompositionElement>().Origin.DisplayName.GetHashCode();
			return result;
		}
	}
}