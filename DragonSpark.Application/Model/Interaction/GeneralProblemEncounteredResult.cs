using System;

namespace DragonSpark.Application.Model.Interaction;

public class GeneralProblemEncounteredResult : UnsuccessfulResultBase
{
	public GeneralProblemEncounteredResult(Exception subject) => Subject = subject;

	public Exception Subject { get; }
}