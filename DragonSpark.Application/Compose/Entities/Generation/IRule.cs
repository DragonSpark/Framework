﻿using Bogus;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public interface IRule<T, out TOther> : ISelect<(Faker, T), TOther> where TOther : class {}
}