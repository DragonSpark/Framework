using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition
{
	public sealed class AppliedExports : DescriptorCollectionBase<AppliedExport>
	{
		public AppliedExports( IEnumerable<AppliedExport> items ) : base( items, descriptor => descriptor.ExportAs ) {}

		protected override Type GetKeyForItem( AppliedExport item ) => item.Subject;

		public IEnumerable<Type> GetClasses() => Get<TypeInfo>();

		public IEnumerable<Type> GetProperties() => Get<PropertyInfo>();

		IEnumerable<Type> Get<T>() where T : MemberInfo => from item in Items where item.Location is T select item.Subject;
	}
}