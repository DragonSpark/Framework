using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication.Entity
{
	public class SearchFilterExpressionFactory<TItem> : FilterExpressionFactoryBase<TItem> where TItem : class
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "Used for convenience." )]
		public static SearchFilterExpressionFactory<TItem> Instance
		{
			get { return InstanceField; }
		}	static readonly SearchFilterExpressionFactory<TItem> InstanceField = new SearchFilterExpressionFactory<TItem>();

		protected override IEnumerable<string> ResolveProperties( Type arg )
		{
			var result = typeof(TItem).GetProperties().Where( x => x.PropertyType == typeof(string) && x.CanWrite && !x.IsDecoratedWith<NotMappedAttribute>() ).Select( x => x.Name ).ToArray();
			return result;
		}
	}
}