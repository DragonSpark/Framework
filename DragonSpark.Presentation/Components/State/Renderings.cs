using DragonSpark.Model.Sequences.Collections;
using System.Collections.Generic;

namespace DragonSpark.Presentation.Components.State;

sealed class Renderings : Membership<IRenderAware>
{
	public Renderings(ICollection<IRenderAware> collection) : base(collection) {}
}