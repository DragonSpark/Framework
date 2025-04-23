using System;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Mobile.Uno.Security.Authentication;

public interface IAuthenticator : ISelecting<Token<Uri>, Uri?>;