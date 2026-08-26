using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Grok.Image;

public interface IGenerateImage : ISelecting<Stop<ImageGenerationInput>, Uri>;