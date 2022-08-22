﻿using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

public interface ICurrentConnection : IResult<HubConnection>, IDisposable {}