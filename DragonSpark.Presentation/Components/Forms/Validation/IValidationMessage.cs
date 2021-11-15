using DragonSpark.Model.Selection;

namespace DragonSpark.Presentation.Components.Forms.Validation;

public interface IValidationMessage<in T> : ISelect<T?, string?> {}