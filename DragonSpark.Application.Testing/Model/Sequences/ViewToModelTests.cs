using DragonSpark.Application.Model.Sequences;
using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Application.Testing.Model.Sequences;

public sealed class ViewToModelTests
{
	[Fact]
	public void Verify()
	{
		var views = new View[]
		{
			new () { Id = 1, Name = "First" },
			new () { Id = 2, Name = "Second" },
			new () { Id = 4, Name = "Fourth" },
			new () { Id = 5, Name = "Fifth" },
		};

		var models = new Model[]
		{
			new () { Id = 3, Name = "Third-Current" },
			new () { Id = 4, Name = "Fourth-Current" },
			new () { Id = 5, Name = "Fifth" },
		};

		using var actions = ViewToModel.Default.Get(new(views, models));
		var (add, update, delete) = actions;
		add.Select(x => x.Id).Should().Equal(1, 2);
		delete.Select(x => x.Id).Should().Equal(3);
		update.Select(x => x.Model.Id).Only().Should().Be(4);
	}

	sealed class ViewToModel : ViewToModel<byte, View, Model>
	{
		public static ViewToModel Default { get; } = new();

		ViewToModel() : base(x => x.Id, x => x.Id, update => update.View.Name != update.Model.Name) {}
	}

	sealed class View
	{
		public byte Id { get; set; }

		public string Name { get; set; } = null!;
	}

	sealed class Model
	{
		public byte Id { get; set; }

		public string Name { get; set; } = null!;
	}
}