namespace Shared.Contracts.CQRS
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }

}
