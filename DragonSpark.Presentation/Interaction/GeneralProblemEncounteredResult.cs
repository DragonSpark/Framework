using System;

namespace DragonSpark.Presentation.Interaction;

public class GeneralProblemEncounteredResult : UnsuccessfulResultBase
{
	public GeneralProblemEncounteredResult(Exception subject) => Subject = subject;

	public Exception Subject { get; }
}