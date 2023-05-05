using Microsoft.AspNetCore.Mvc;

namespace DragonSpark.Server.Requests;

public readonly record struct Request<T>(ControllerBase Owner, Unique<T> Parameter);