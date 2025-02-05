using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands
{
    public class DeletePostCommand : BaseCommand
    {
        // lets check if commits will be only for 1
        public string UserName{get;set;}
    }
}