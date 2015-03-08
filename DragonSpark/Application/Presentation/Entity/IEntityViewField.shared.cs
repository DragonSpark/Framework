using System;
using DragonSpark.Application.Communication.Entity;
using DragonSpark.Objects;

namespace DragonSpark.Application.Presentation.Entity
{
	public interface IEntityViewField : IAuthorizable, IItemProfile
	{
		string FieldName { get; }

		bool IsViewable { get; }

		bool IsEditable { get; }
	}
}