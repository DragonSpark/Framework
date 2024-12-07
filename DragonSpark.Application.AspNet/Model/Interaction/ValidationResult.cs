namespace DragonSpark.Application.Model.Interaction;

public record ValidationResult(string Message) : IInteractionResult;