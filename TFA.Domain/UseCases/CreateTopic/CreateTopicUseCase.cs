using Microsoft.EntityFrameworkCore;
using TFA.Domain.Exceptions;
using TFA.Domain.Factory;
using TFA.Domain.Provider;
using TFA.Storage;
using Topic = TFA.Domain.Models.Topic;

namespace TFA.Domain.UseCases.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly ForumDbContext _dbContext;
    private readonly IGuidFactory _guidFactory;
    private readonly IMomentProvider _momentProvider;

    public CreateTopicUseCase(ForumDbContext dbContext, IGuidFactory guidFactory, IMomentProvider momentProvider)
    {
        _dbContext = dbContext;
        _guidFactory = guidFactory;
        _momentProvider = momentProvider;
    }

    public async Task<Topic> Execute(Guid forumId, string title, Guid authorId, CancellationToken cancellationToken)
    {
        var topicExists =
            await _dbContext.Topics.AnyAsync(i => i.ForumId == forumId, cancellationToken: cancellationToken);
        if (!topicExists)
        {
            throw new ForumNotFoundException(forumId);
        }

        var topicId = _guidFactory.Create();
        await _dbContext.Topics.AddAsync(new Storage.Topic()
        {
            TopicId = topicId,
            ForumId = forumId,
            CreateAt = _momentProvider.Now,
            UserId = authorId,
            Title = "Hello world"
        }, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await _dbContext.Topics.Where(t => t.TopicId == topicId).Select(t => new Topic()
        {
            Id = t.TopicId,
            Title = t.Title,
            CreatedAt = t.CreateAt,
            Author = t.Author.Login
        }).FirstAsync(cancellationToken);
    }
}