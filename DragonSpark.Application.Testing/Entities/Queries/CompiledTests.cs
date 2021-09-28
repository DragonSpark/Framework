using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Testing.Entities.Queries
{
	public sealed class CompiledTests
	{
		/*[Fact]
		public void Verify() {}

		[Fact]
		public async Task Verify()
		{
			await using var contexts = await new SqlContexts<ContextWithData>().Initialize();
			{
				await using var context = contexts.Get();
				await context.Database.EnsureCreatedAsync();
			}
		}*/

		public sealed class ContextWithData : DbContext
		{
			public ContextWithData(DbContextOptions options) : base(options) {}

			[UsedImplicitly]
			public DbSet<Subject> Subjects { get; set; } = default!;

			protected override void OnModelCreating(ModelBuilder modelBuilder)
			{
				base.OnModelCreating(modelBuilder);
				modelBuilder.Entity<Subject>()
				            .HasData(new Subject { Id = new Guid("C750443A-19D0-4FD0-B45A-9D1722AD0DB3"), Name = "One" },
				                     new Subject
				                     {
					                     Id = new Guid("08013B99-3297-49F6-805E-0A94AE5B79A2"), Name = "Two"
				                     },
				                     new Subject
				                     {
					                     Id = new Guid("5559B8B9-1F19-4F91-A6C8-DE3BB5E47603"), Name = "Three"
				                     });
			}
		}

		public sealed class Subject
		{
			[UsedImplicitly]
			public Guid Id { get; set; }

			[UsedImplicitly]
			public string Name { get; set; } = default!;

			[UsedImplicitly]
			public int Number { get; set; }
		}

		/*sealed class SelectSubjectsByName : StartWhere<string, Subject>
		{
			public static SelectSubjectsByName Default { get; } = new SelectSubjectsByName();

			SelectSubjectsByName() : base((s, subject) => subject.Name == s) {}
		}*/

		/*sealed class SingleSubject : EvaluateToSingle<string, Subject>
		{
			public SingleSubject(IContexts<ContextWithData> contexts)
				: base(contexts.Then().Scopes(), SelectSubjectsByName.Default) {}
		}*/

		/*public class Benchmarks
		{
			readonly IContexts<ContextWithData>      _contexts;
			readonly IRuntimeQuery<string, Subject>       _scoped, _singleton;
			readonly IMaterializer<Subject, Subject> _evaluate;
			readonly ISelecting<string, Subject>     _single;

			public Benchmarks() : this(new PooledSqlContexts<ContextWithData>(), SelectSubjectsByName.Default) {}

			public Benchmarks(IContexts<ContextWithData> contexts, IQuery<string, Subject> query)
				: this(contexts, contexts.Get().Then().Use(query).Compile(), contexts.Then().Use(query).Compile(),
				       SingleMaterializer<Subject>.Default, new SingleSubject(contexts)) {}

			// ReSharper disable once TooManyDependencies
			public Benchmarks(IContexts<ContextWithData> contexts, IRuntimeQuery<string, Subject> scoped,
			                  IRuntimeQuery<string, Subject> singleton, IMaterializer<Subject, Subject> evaluate,
			                  ISelecting<string, Subject> single)
			{
				_contexts  = contexts;
				_scoped    = scoped;
				_singleton = singleton;
				_evaluate  = evaluate;
				_single    = single;
			}

			[GlobalSetup]
			public async Task GlobalSetup()
			{
				await using var context = _contexts.Get();
				await context.Database.EnsureCreatedAsync();
			}

			[GlobalCleanup]
			public async Task GlobalCleanup()
			{
				await using var context = _contexts.Get();
				await context.Database.EnsureDeletedAsync();
			}

			[Benchmark(Baseline = true)]
			public async ValueTask<object> Scoped()
			{
				var       queries = _scoped.Get("One");
				using var session = await queries.Get();
				var       result  = await _evaluate.Await(session.Subject);
				return result;
			}

			[Benchmark]
			public async ValueTask<object> Singleton()
			{
				var       queries = _singleton.Get("One");
				using var session = await queries.Get();
				var       result  = await _evaluate.Await(session.Subject);
				return result;
			}

			[Benchmark]
			public async ValueTask<object> Single() => await _single.Await("One");
		}*/
	}
}