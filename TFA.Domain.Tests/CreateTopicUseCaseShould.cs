using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Language.Flow;
using TFA.Domain.Exceptions;
using TFA.Domain.Factory;
using TFA.Domain.Provider;
using TFA.Domain.UseCases.CreateTopic;
using TFA.Storage;
using Topic = TFA.Storage.Topic;

namespace TFA.Domain.Tests;

public class CreateTopicUseCaseShould
{
    private readonly CreateTopicUseCase _sut;
    private readonly ForumDbContext _forumDbContext;
    private readonly ISetup<IGuidFactory, Guid> _createIdSetup;
    private readonly ISetup<IMomentProvider, DateTimeOffset> _getNowSetup;

    public CreateTopicUseCaseShould()
    {
        var dbContextOptionsBuilder =
            new DbContextOptionsBuilder().UseInMemoryDatabase(nameof(CreateTopicUseCaseShould));
        _forumDbContext = new ForumDbContext(dbContextOptionsBuilder.Options);
        
        var guidFactory = new Mock<IGuidFactory>();
        _createIdSetup = guidFactory.Setup(f => f.Create());
        var momentProvider = new Mock<IMomentProvider>();
        _getNowSetup = momentProvider.Setup(p => p.Now);
        
        _sut = new CreateTopicUseCase(_forumDbContext, guidFactory.Object, momentProvider.Object);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoForum()
    {
        await _forumDbContext.Forums.AddAsync(new Forum
        {
            ForumId = Guid.Parse("2cfb36e3-7a03-1225-9f07-afbaf0988e12"),
            Title = "Basic forum"
        });

        await _forumDbContext.SaveChangesAsync();
        var forumId = Guid.Parse("2cfb36e3-7a03-4912-9f07-afbaf0988e12");
        var authorId = Guid.Parse("810243a4-b9e4-4417-add4-a1c429e81411");
        await _sut.Invoking(s => s.Execute(forumId, "Some", authorId, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnNewCreateTopic()
    {
        var forumId = Guid.Parse("2eddec72-6cd0-44d6-84e2-f257aeff1744");
        var userId = Guid.Parse("21a2c4bd-8a9d-4289-b144-59aa4c1298fe");

        await _forumDbContext.Forums.AddAsync(new Forum
        {
            ForumId = forumId,
            Title = "Existing forum"
        });
        
        await _forumDbContext.Users.AddAsync(new User
        {
            UserId = userId,
            Login = "Anton",
        });
        await _forumDbContext.SaveChangesAsync();
        _createIdSetup.Returns(new Guid("CA931610-BA75-41F5-B697-84799A438387"));
        _getNowSetup.Returns(new DateTimeOffset(2023, 09, 17, 19,08, 00, TimeSpan.FromHours(2)));
        
        var actual = await _sut.Execute(forumId, "Hello world", userId, CancellationToken.None);
        var allTopics = await _forumDbContext.Topics.ToArrayAsync();

        allTopics.Should().BeEquivalentTo(new[]
        {
            new Topic
            {
                ForumId = forumId,
                UserId = userId,
                Title = "Hello world"
            }
        }, cnf => cnf.Including(t => t.UserId).Including(t => t.TopicId).Including(t => t.Title));

        actual.Should().BeEquivalentTo(new Models.Topic
        {
            Id = Guid.Parse("86CB19A8-FDD9-42B7-AA60-49C65E776C4A"),
            Title = "Hello world",
            CreatedAt = DateTimeOffset.Now,
            Author = "Anton"
        });
    }
}