using System;
using System.Text.Json.Serialization;

namespace DragonSpark.Contracts.Worker;

[JsonDerivedType(typeof(ProcessStatusView), 0), JsonDerivedType(typeof(SuccessStatusView), 1)]
public record ProcessStatusView(ProcessStatus Status, DateTimeOffset Time, string? Message);