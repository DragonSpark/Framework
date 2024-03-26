using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Editing;
using DragonSpark.Application.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects.Entities.SqlLite;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Queries;

public sealed class DemonstrationTests
{
	[Fact]
	public async Task Verify()
	{
		int             id;
		await using var contexts = await new SqlLiteNewContext<BloggingContext>().Initialize();
		var scopes = new EnlistedScopes(new Scopes<BloggingContext>(contexts.NewContext),
		                                AmbientContext.Default);
		var editors = new Editors(scopes);
		var get     = new GetSpecificBlog(scopes);
		var edit    = new EditSpecificBlog(scopes);
		{
			using var editor = editors.Get();
			var       entity = new Blog { Url = "http://blogs.msdn.com/adonet" };
			editor.Add(entity);
			await editor.Await();
			id = entity.BlogId;
		}

		id.Should().NotBe(0);

		{
			using var editor = await edit.Await(id);
			editor.Subject.Posts.Add(new() { Title = "Hello World", Content = "I wrote an app using EF Core!" });
			await editor.Await();
		}

		{
			var specific = await get.Await(id);
			specific.Should().NotBeNull();
			var verify = specific.Verify();
			verify.BlogId.Should().Be(id);
			verify.Posts.Should().NotBeEmpty();
		}

		{
			using var editor = await edit.Await(id);
			editor.Remove(editor.Subject);
			await editor.Await();
		}

		{
			var specific = await get.Await(id);
			specific.Should().BeNull();
		}
	}

	sealed class EditSpecificBlog : Editing<int, Blog>
	{
		public EditSpecificBlog(IScopes scope) : base(scope, SelectBlogs.Default) {}
	}

	sealed class GetSpecificBlog : EvaluateToSingleOrDefault<int, Blog>
	{
		public GetSpecificBlog(IScopes scope) : base(scope, SelectBlogs.Default) {}
	}

	sealed class SelectBlogs : StartWhere<int, Blog>
	{
		public static SelectBlogs Default { get; } = new();

		SelectBlogs() : base(q => q.Include(x => x.Posts), (p, x) => x.BlogId == p) {}
	}

	public class BloggingContext : DbContext
	{
		public BloggingContext(DbContextOptions options) : base(options) {}

		public DbSet<Blog> Blogs { get; set; } = default!;

		public DbSet<Post> Posts { get; set; } = default!;
	}

	public class Blog
	{
		public int BlogId { get; set; }
		[MaxLength(1024)]
		public string Url { get; set; } = default!;

		public List<Post> Posts { get; } = new();
	}

	public class Post
	{
		public int PostId { get; set; }
		[MaxLength(64)]
		public string Title { get; set; } = default!;
		[MaxLength(1024)]
		public string Content { get; set; } = default!;

		public int BlogId { get; set; }
		public Blog Blog { get; set; } = default!;
	}
}