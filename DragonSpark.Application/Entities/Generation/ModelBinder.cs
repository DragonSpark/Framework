using AutoBogus;
using DragonSpark.Compose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Application.Entities.Generation
{
	sealed class ModelBinder<T> : ModelBinder
	{
		public static ModelBinder<T> Default { get; } = new ModelBinder<T>();

		ModelBinder() : base(A.Type<T>().Assembly) {}

	}

	public class ModelBinder : IAutoBinder
	{
		readonly Func<MemberInfo, bool> _filter;
		readonly IAutoBinder            _previous;

		public ModelBinder(Assembly modelAssembly) : this(Start.A.Selection<PropertyInfo>()
		                                                       .By.Calling(x => x.PropertyType)
		                                                       .Select(MemberAssembly.Default)
		                                                       .Select(Is.EqualTo(modelAssembly))
		                                                       .Out()
		                                                       .Then()
		                                                       .Inverse()) {}

		public ModelBinder(Func<PropertyInfo, bool> filter)
			: this(Start.A.Selection<MemberInfo>()
			            .By.Cast<PropertyInfo>()
			            .Select(filter)
			            .Ensure.Input.IsOf<PropertyInfo>()
			            .Otherwise.Use(_ => true),
			       new AutoBinder()) {}

		public ModelBinder(Func<MemberInfo, bool> filter, IAutoBinder previous)
		{
			_filter   = filter;
			_previous = previous;
		}

		public Dictionary<string, MemberInfo> GetMembers(Type t) => _previous.GetMembers(t);

		public TType CreateInstance<TType>(AutoGenerateContext context) => _previous.CreateInstance<TType>(context);

		public void PopulateInstance<TType>(object instance, AutoGenerateContext context,
		                                    IEnumerable<MemberInfo>? members = null)
		{
			_previous.PopulateInstance<TType>(instance, context, members?.Where(_filter));
		}
	}
}