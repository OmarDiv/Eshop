namespace Shared.CQRS
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }

}
