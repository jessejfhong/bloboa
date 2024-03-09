using MediatR;

namespace LooperCorp.Application;

public abstract class MenuAction : IRequest {}
public abstract class MenuAction<T> : IRequest<T> {}
