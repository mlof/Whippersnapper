using MediatR;
using Microsoft.EntityFrameworkCore;
using Whippersnapper.Data;

namespace Whippersnapper.Modules.Administration;

public class TopUsersHandler : IRequestHandler<TopUsersRequest, TopUsersResponse>
{
    private readonly WhippersnapperContext _dbContext;

    public TopUsersHandler(WhippersnapperContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TopUsersResponse> Handle(TopUsersRequest request, CancellationToken cancellationToken)
    {
        var topAuthors = await _dbContext.Transcriptions
            .Where(transcription => transcription.GuildId == request.GuildId)
            .GroupBy(x => x.Author)
            .Select(x => new UserUsage { Author = x.Key, Count = x.Count() })
            .OrderByDescending(x => x.Count).Take(10).ToListAsync(cancellationToken: cancellationToken);


        return new TopUsersResponse()
        {
            Authors = topAuthors
        };
    }
}