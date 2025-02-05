using CQRS.Core.Commands;

namespace CQRS.Core.Infrastructure
{
    public interface ICommandDispetcher
    {
        void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;
        Task SendAsync(BaseCommand command);
    }
}