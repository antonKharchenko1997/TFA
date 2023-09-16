using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage;

public class Comment
{
    [Key] public Guid CommentId { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public string Text { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
    [ForeignKey(nameof(UserId))] public User Author { get; set; }
    [ForeignKey(nameof(TopicId))] public Topic Topic { get; set; }
}