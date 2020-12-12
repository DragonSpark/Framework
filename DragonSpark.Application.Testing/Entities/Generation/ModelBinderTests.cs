using AutoBogus;
using AutoBogus.Conventions;
using DragonSpark.Application.Entities.Generation;
using DragonSpark.Compose;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Generation
{
	public sealed class ModelBinderTests
	{
		public ModelBinderTests() : this(ModelBinder<Parent>.Default) {}

		readonly IAutoBinder _binder;

		ModelBinderTests(IAutoBinder binder) => _binder = binder;

		[Fact]
		public void Verify()
		{
			var context = new AutoFaker<Parent>().Configure(x => x.WithConventions().WithBinder(_binder));
			var subject = context.Generate();

			subject.Child.Should().BeNull();
			subject.Name.Should().NotBeNullOrEmpty();
		}

		[Fact]
		public void VerifyRelationship()
		{
			var child = new AutoFaker<Child>().Configure(x => x.WithConventions().WithBinder(_binder));
			var parent = new AutoFaker<Parent>().Configure(x => x.WithConventions().WithBinder(_binder))
			                                    .RuleFor(x => x.Child, child);
			var subject = parent.Generate();

			subject.Child.Should().NotBeNull();
			subject.Child.Parent.Should().BeNull();
			subject.Child.Name.Should().NotBeNullOrEmpty();
			subject.Name.Should().NotBeNullOrEmpty();
		}

		[Fact]
		public void VerifyBasicModel()
		{
			var subject = Start.A.Generator<Parent>().Include(x => x.Child).Get();
			subject.Child.Should().NotBeNull();
			subject.Child.Parent.Should().NotBeNull();
			subject.Child.Parent.Should().BeSameAs(subject);
			subject.Child.Name.Should().NotBeNullOrEmpty();
			subject.Name.Should().NotBeNullOrEmpty();
		}

		sealed class Parent
		{
			[UsedImplicitly]
			public Guid Id { get; set; }

			[UsedImplicitly]
			public bool Enabled { get; set; } = true;

			[UsedImplicitly]
			public DateTimeOffset Created { get; set; }

			[Required, MaxLength(128)]
			public string Name { get; set; } = default!;

			[MaxLength(1024), UsedImplicitly]
			public string? Description { get; set; } = string.Empty;

			[Column(TypeName = "decimal(32,16)"), Range(1, 10_000), UsedImplicitly]
			public decimal Price { get; set; }

			[UsedImplicitly]
			public Child Child { get; set; } = default!;
		}

		sealed class Child
		{
			[UsedImplicitly]
			public Guid Id { get; set; }

			[UsedImplicitly]
			public bool Enabled { get; set; } = true;

			[UsedImplicitly]
			public DateTimeOffset Created { get; set; }

			[Required, MaxLength(128), UsedImplicitly]
			public string Name { get; set; } = default!;

			[MaxLength(1024), UsedImplicitly]
			public string? Description { get; set; } = string.Empty;

			[Column(TypeName = "decimal(32,16)"), Range(1, 10_000), UsedImplicitly]
			public decimal Price { get; set; }

			[UsedImplicitly]
			public Parent Parent { get; set; } = default!;
		}
	}
}