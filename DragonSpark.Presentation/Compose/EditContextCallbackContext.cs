﻿using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Presentation.Compose;

public sealed class EditContextCallbackContext
{
	readonly EditContext _context;

	public EditContextCallbackContext(EditContext context) => _context = context;

	public CallbackContext Field() => Field(new FieldIdentifier());

	public CallbackContext Field(string field) => Field(_context.Field(field));

	public CallbackContext Field<T>(Expression<Func<T>> expression) => Field(FieldIdentifier.Create(expression));

	public CallbackContext Field(in FieldIdentifier field)
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