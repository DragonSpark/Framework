﻿using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;

namespace DragonSpark.Application
{
	public sealed class ApplicationParts : Scope<SystemParts?>
	{
		public static IScope<SystemParts?> Default { get; } = new ApplicationParts();
		ApplicationParts() : base( () => null ) {}

		// public static bool IsAssigned => !Equals( Default.Get(), SystemParts.Default );
	}

	public sealed class ApplicationPartsAssignedSpecification : SuppliedDelegatedSpecification<SystemParts?>
	{
		public static ISpecification<object> Default { get; } = new ApplicationPartsAssignedSpecification();
		ApplicationPartsAssignedSpecification() : base( AssignedSpecification<SystemParts?>.Default, ApplicationParts.Default.Get ) {}
	}
}