using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities
{
	public interface IModifySchema : ISelect<Type, IAlteration<ModelBuilder>?> {}

}