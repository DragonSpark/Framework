using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExpressionVisitor = System.Linq.Expressions.ExpressionVisitor;

namespace DragonSpark.Application.Entities.Queries
{
	class Class5 {}

	/*public class Update<TIn, T> : ISelecting<TIn, T> where T : class
	{
		readonly IQuery<TIn, T>      _query;
		readonly Action<T>           _update;
		readonly IMaterializer<T, T> _materializer;

		protected Update(IQuery<TIn, T> query, Action<T> update) : this(query, update, SingleMaterializer<T>.Default) {}

		protected Update(IQuery<TIn, T> query, Action<T> update, IMaterializer<T, T> materializer)
		{
			_query        = query;
			_update       = update;
			_materializer = materializer;
		}

		public async ValueTask<T> Get(TIn parameter)
		{
			await using var session = _query.Get(parameter);
			var (context, subject) = session;
			var result = await _materializer.Await(subject);
			_update(result);
			await context.SaveChangesAsync().ConfigureAwait(false);
			return result;
		}
	}*/

	sealed class ParameterExtractor<T> : ISelect<LambdaExpression, ParameterExpression?>
	{
		public static ParameterExtractor<T> Default { get; } = new ParameterExtractor<T>();

		ParameterExtractor() : this(Is.EqualTo(A.Type<T>()).Out()) {}

		readonly ICondition<Type> _condition;

		public ParameterExtractor(ICondition<Type> condition) => _condition = condition;

		public ParameterExpression? Get(LambdaExpression parameter)
		{
			for (byte i = 0; i < parameter.Parameters.Count; i++)
			{
				var expression = parameter.Parameters[i];
				if (_condition.Get(expression.Type))
				{
					return expression;
				}
			}

			return null;
		}
	}

	sealed class ParameterUsageRewriterState
	{
		readonly IReadOnlyCollection<ParameterExpression> _parameters;
		readonly ParameterExpression                      _parameter;
		readonly IOrderedDictionary<string, Replacement>  _located;

		public ParameterUsageRewriterState(IReadOnlyCollection<ParameterExpression> parameters)
			: this(parameters, parameters.Last()) {}

		public ParameterUsageRewriterState(IReadOnlyCollection<ParameterExpression> parameters,
		                                   ParameterExpression parameter)
			: this(parameters, parameter, new OrderedDictionary<string, Replacement>()) {}

		public ParameterUsageRewriterState(IReadOnlyCollection<ParameterExpression> parameters,
		                                   ParameterExpression parameter,
		                                   IOrderedDictionary<string, Replacement> located)
		{
			_parameters = parameters;
			_parameter  = parameter;
			_located    = located;
		}

		public bool Encountered { get; set; }

		bool Active { get; set; }

		bool Monitor { get; set; }

		public void Start()
		{
			if (Active)
			{
				throw new
					InvalidOperationException("An attempt to start was made, but this instance is already active.");
			}

			_located.Clear();
			Active      = true;
			Encountered = false;
			Monitor     = true;
		}

		public void Body()
		{
			Monitor = false;
		}

		public void Accept(ParameterExpression expression)
		{
			Encountered |= Monitor && expression == _parameter;
		}

		public Expression? Accept(MemberExpression expression)
		{
			if (Root.Default.Get(expression) == _parameter)
			{
				var key = expression.ToString();
				return _located.TryGetValue(key, out var existing) ? existing.Parameter : Add(key, expression);
			}

			return null;
		}

		ParameterExpression Add(string key, MemberExpression expression)
		{
			var result    = Expression.Parameter(expression.Type, $"{_parameter.Name}_parameter_{_located.Count}");
			var @delegate = Expression.Lambda(expression, _parameter.Yield()).Compile();
			_located.Add(key, new(expression.Type, @delegate, result));
			return result;
		}

		public RewriteResult Complete(LambdaExpression expression)
		{
			try
			{
				var start        = Encountered ? _parameters : _parameters.Except(_parameter.Yield());
				var replacements = _located.Values.AsValueEnumerable();
				var lambda = Expression.Lambda(expression.Body,
				                               start.Concat(replacements.Select(x => x.Parameter).ToArray()));
				var others = replacements.Select(x => x.Delegate).ToArray();
				var delegates = Encountered
					                ? others.Prepend(Expression.Lambda(_parameter, _parameter.Yield()).Compile())
					                        .ToArray()
					                : others;
				var types = Encountered
					            ? _located.Values.Select(x => x.ResultType).Prepend(_parameter.Type).ToArray()
					            : replacements.Select(x => x.ResultType).ToArray();
				return new RewriteResult(lambda, types, delegates);
			}
			finally
			{
				_located.Clear();
				Active = false;
			}
		}
	}

	public readonly record struct RewriteResult(LambdaExpression Expression, Array<Type> Types,
	                                            Array<Delegate> Delegates);

	public readonly record struct Replacement(Type ResultType, Delegate Delegate, ParameterExpression Parameter);

	sealed class Root : ISelect<MemberExpression, Expression>
	{
		public static Root Default { get; } = new Root();

		Root() {}

		public Expression Get(MemberExpression parameter)
		{
			var current = parameter;
			while (current.Expression is MemberExpression next)
			{
				current = next;
			}

			return current.Expression.Verify();
		}
	}

	sealed class ParameterUsageRewriter : ExpressionVisitor
	{
		readonly LambdaExpression            _subject;
		readonly ParameterUsageRewriterState _state;

		public ParameterUsageRewriter(LambdaExpression subject)
			: this(subject, new ParameterUsageRewriterState(subject.Parameters)) {}

		public ParameterUsageRewriter(LambdaExpression subject, ParameterUsageRewriterState state)
		{
			_subject = subject;
			_state   = state;
		}

		public RewriteResult Rewrite()
		{
			_state.Start();

			var expression = Visit(_subject).Verify().To<LambdaExpression>();
			var result     = _state.Complete(expression);
			return result;
		}

		public override Expression? Visit(Expression? node)
		{
			var body   = node == _subject.Body;
			var result = base.Visit(node);
			if (body)
			{
				_state.Body();
			}

			return result;
		}

		protected override Expression VisitParameter(ParameterExpression node)
		{
			_state.Accept(node);
			return base.VisitParameter(node);
		}

		protected override Expression VisitMember(MemberExpression node)
			=> _state.Accept(node) ?? base.VisitMember(node);
	}
}