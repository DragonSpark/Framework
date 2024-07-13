using DragonSpark.Model.Selection;
using SixLabors.ImageSharp;

namespace DragonSpark.Drawing;

sealed class PositionCenter : ISelect<PositionCenterInput, Point>
{
	public static PositionCenter Default { get; } = new();

	PositionCenter() {}

	public Point Get(PositionCenterInput parameter)
	{
		var ((w, h), (x, y, width, height)) = parameter;
		return new(x + width / 2 - w / 2, y + height / 2 - h / 2);
	}
}