using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Sequences.Collections.Groups;

class DefaultAddGroupElementCommand<T> : Command<T>
{
	public DefaultAddGroupElementCommand(GroupName defaultName, IConditional<string, GroupName> names,
	                                     IGroupCollection<T> collection)
		: base(new AddGroupElementCommand<T>(collection, new GroupName<T>(defaultName, names))
		       .Then()
		       .Selection()
		       .Unless.Input.IsOf(new GroupingAwareCommand<T>(collection).Then().Selection().Get())
		       .ToCommand()) {}
}