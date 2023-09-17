using MediatR;

namespace Whippersnapper.Modules.Administration;

public class TopUsersRequest : IRequest<TopUsersResponse>
{
    public TopUsersRequest(ulong guildId)
    {
        GuildId = guildId;
    }

    public ulong GuildId { get; }
}