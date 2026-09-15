using DragonSpark.Model.Operations.Selection.Stop;
using SixLabors.ImageSharp;

namespace DragonSpark.Drawing;

public interface ILoadImage : IStopAware<LoadImageInput, Image>;