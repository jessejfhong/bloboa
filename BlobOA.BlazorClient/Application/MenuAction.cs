using MediatR;

namespace BlobOA.BlazorClient.Application;

public abstract class MenuAction : IRequest {}
public abstract class MenuAction<T> : IRequest<T> {}
