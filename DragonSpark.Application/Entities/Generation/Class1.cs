using AutoBogus;
using DragonSpark.Compose;
using DragonSpark.Reflection.Collections;
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
			            .Otherwise.Use(info => true),
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

	sealed class MemberAssembly : DragonSpark.Model.Selection.Coalesce<Type, Assembly>
	{
		public static MemberAssembly Default { get; } = new MemberAssembly();

		MemberAssembly() : base(Start.A.Selection.Of.System.Type.By.Self.Then()
		                             .Metadata()
		                             .Select(CollectionInnerType.Default)
		                             .Select(x => x?.Assembly), x => x.Assembly) {}
	}

	/*
	public sealed class ModelBinder : IAutoBinder
	{
		readonly Func<MemberInfo, bool> _filter;
		readonly IAutoBinder            _previous;
		readonly IInstances             _instances;

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
			            .Otherwise.Use(info => true),
			       new AutoBinder()) {}

		public ModelBinder(Func<MemberInfo, bool> filter, IAutoBinder previous)
			: this(filter, previous, new Instances(previous)) {}

		public ModelBinder(Func<MemberInfo, bool> filter, IAutoBinder previous, IInstances instances)
		{
			_filter    = filter;
			_previous  = previous;
			_instances = instances;
		}

		public Dictionary<string, MemberInfo> GetMembers(Type t) => _previous.GetMembers(t);

		public TType CreateInstance<TType>(AutoGenerateContext context)
		{
			var instance = _instances.Get((context, A.Type<TType>()));
			var result   = instance is TType type ? type : default!;
			return result;
		}

		public void PopulateInstance<TType>(object instance, AutoGenerateContext context,
		                                    IEnumerable<MemberInfo> members)
		{
			if (_instances.Get(instance))
			{
				_previous.PopulateInstance<TType>(instance, context, members);
			}
		}

		public interface IInstances : ICondition<object>, ISelect<(AutoGenerateContext, Type), object> {}

		sealed class Instances : Condition<object>, IInstances
		{
			readonly ITypedTable<object> _store;
			readonly IObjects            _objects;

			public Instances(IAutoBinder binder) : this(new TypedTable<object>(), new Objects(binder)) {}

			public Instances(ITypedTable<object> store, IObjects objects)
				: base(Is.Assigned().And(new HashSet<object>().Add))
			{
				_store   = store;
				_objects = objects;
			}

			public object Get((AutoGenerateContext, Type) parameter)
			{
				var (_, requested) = parameter;
				return _store.TryGet(requested.GetTypeInfo(), out var existing)
					       ? existing
					       : Store(parameter);
			}

			object Store((AutoGenerateContext, Type) parameter)
			{
				var (_, requested) = parameter;
				var result = _objects.Get(parameter);
				_store.Assign(requested.GetTypeInfo(), result);
				return result;
			}
		}

		public interface IObjects : ISelect<(AutoGenerateContext, Type), object> {}

		sealed class Objects : IObjects
		{
			readonly IGeneric<IAutoBinder, AutoGenerateContext, IResult<object>> _inner;
			readonly IAutoBinder                                                 _binder;

			public Objects(IAutoBinder binder) : this(Invoker.Default, binder) {}

			public Objects(IGeneric<IAutoBinder, AutoGenerateContext, IResult<object>> inner, IAutoBinder binder)
			{
				_inner  = inner;
				_binder = binder;
			}

			public object Get((AutoGenerateContext, Type) parameter)
				=> _inner.Get(parameter.Item2)(_binder, parameter.Item1).Get();
		}

		sealed class Invoker : Generic<IAutoBinder, AutoGenerateContext, IResult<object>>
		{
			public static Invoker Default { get; } = new Invoker();

			Invoker() : base(typeof(Invoker<>)) {}
		}

		sealed class Invoker<T> : IResult<object?>
		{
			readonly IAutoBinder         _binder;
			readonly AutoGenerateContext _context;

			public Invoker(IAutoBinder binder, AutoGenerateContext context)
			{
				_binder  = binder;
				_context = context;
			}

			public object? Get() => _binder.CreateInstance<T>(_context);
		}
	}
*/
}