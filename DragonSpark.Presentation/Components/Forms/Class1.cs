using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Presentation.Components.Forms;

class Class1;

public sealed record LoadEditContextMessage(EditContext Subject);
public sealed record SaveEditContextMessage(EditContext Subject);

public sealed record LoadModelMessage(object Subject);
public sealed record SaveModelMessage(object Subject);