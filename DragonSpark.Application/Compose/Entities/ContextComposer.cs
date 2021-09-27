﻿using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Editing;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Model;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class ContextComposer
	{
		readonly DbContext _subject;

		public ContextComposer(DbContext subject) => _subject = subject;

		public IScopes Scopes() => new SessionScopes(_subject);

		public IEditor Editor() => new Editor(_subject);

		public ScopesComposer<None, TElement> Use<TElement>() where TElement : class
			=> new(Scopes(), Set<TElement>.Default);

		public ScopesComposer<TIn, TElement> Use<TIn, TElement>(IQuery<TIn, TElement> query) => new(Scopes(), query);
	}
}