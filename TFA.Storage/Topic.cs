using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage;

public class Topic
{
    [Key] public Guid TopicId { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid ForumId { get; set; }

    [ForeignKey(nameof(UserId))] public User Author { get; set; }
    [ForeignKey(nameof(ForumId))] public Forum Forum { get; set; }
    
    [InverseProperty(nameof(Comment.TopicId))]
    private ICollection<Comment> Comments { get; set; }
}