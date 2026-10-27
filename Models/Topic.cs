using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoangViet.Models;

public class Topic
{
    [Key]
    public int Id { get; set; }

    public int TrackId { get; set; }

    [ForeignKey(nameof(TrackId))]
    public Track? Track { get; set; }

    public int TopicNumber { get; set; }

    [Required]
    [MaxLength(250)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(600)]
    public string Description { get; set; } = string.Empty;

    public ICollection<SubLesson> SubLessons { get; set; } = new List<SubLesson>();
}
