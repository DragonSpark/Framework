using DragonSpark.Compose;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class ParameterUsageEditor : ExpressionVisitor
{
	readonly LambdaExpression        _subject;
	readonly ParameterUsageEditState _state;

	public ParameterUsageEditor(LambdaExpression subject)
		: this(subject, new ParameterUsageEditState(subject.Parameters)) {}

	public ParameterUsageEditor(LambdaExpression subject, ParameterUsageEditState state)
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