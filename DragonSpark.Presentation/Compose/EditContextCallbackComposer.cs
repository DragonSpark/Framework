using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Presentation.Compose;

public sealed class EditContextCallbackComposer
{
	readonly EditContext _context;

	public EditContextCallbackComposer(EditContext context) => _context = context;

	public CallbackComposer Field() => Field(new FieldIdentifier());

	public CallbackComposer Field(string field) => Field(_context.Field(field));

	public CallbackComposer Field<T>(Expression<Func<T>> expression) => Field(FieldIdentifier.Create(expression));

	public CallbackComposer Field(in FieldIdentifier field)
		=> new(new NotifyField(_context, in field).Then().Operation().Allocate());

	sealed class NotifyField : ICommand
	{
		readonly EditContext     _context;
		readonly FieldIdentifier _identifier;

		public NotifyField(EditContext context, in FieldIdentifier identifier)
		{
			_context    = context;
			_identifier = identifier;
		}

		public void Execute(None parameter)
		{
			_context.NotifyFieldChanged(in _identifier);
		}
	}
}