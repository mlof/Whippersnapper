using MediatR;

namespace Whippersnapper.Modules.Administration;

public class TopUsersRequest : IRequest<TopUsersResponse>
{
    public ulong GuildId { get; }

    public TopUsersRequest(ulong guildId)
    {
        GuildId = guildId;
    }
}