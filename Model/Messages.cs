using Supabase;
using Postgrest.Attributes;

namespace AvaloniaDatabase.Model;

public class Messages : SupabaseModel
{
    [PrimaryKey("id", false)]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("message")]
    public string Message { get; set; } = string.Empty;

    [Column("created_at")]
    public string CreatedAt { get; set; } = string.Empty;
}