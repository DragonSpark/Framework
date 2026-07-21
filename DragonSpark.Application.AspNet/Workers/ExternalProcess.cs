using System;
using System.Collections.Generic;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Queries.Compiled.Evaluation;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Workers;

[Index(nameof(Created), IsDescending = [true])]
public abstract class ExternalProcess
{
	public Guid Id { get; set; }

	public bool Enabled { get; set; } = true;

	public DateTimeOffset Created { get; set; }

	public DateTimeOffset? Completed { get; set; }

	public ICollection<CompletedStep> CompletedSteps { get; init; } = null!;

	public ICollection<ProcessUpdate> Updates { get; set; } = null!;

	public ProcessState State { get; set; } = null!;
}
// TODO
public sealed class LocateExternalProcessReference : Locate<Guid, ExternalProcess>
{
	public LocateExternalProcessReference(IEnlistedScopes scopes)
		: base(scopes, x => x.Id, SelectExternalProcess.Default) {}
}

public sealed class EvaluateExternalProcessReference : EvaluateToSingle<Guid, ExternalProcess>
{
	public EvaluateExternalProcessReference(IEnlistedScopes scopes) : base(scopes, SelectExternalProcess.Default) {}
}

sealed class SelectExternalProcess : StartWhere<Guid, ExternalProcess>
{
	public static SelectExternalProcess Default { get; } = new();

	SelectExternalProcess() : base((p, x) => x.Id == p) {}
}
