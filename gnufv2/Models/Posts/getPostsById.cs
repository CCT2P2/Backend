namespace Gnuf.Models.Posts;

public class PostByIdQueryParameters
{
    public string? ids { get; set; }
}
public class GetPostsByIdResponse
{
    public List<PostStructure> posts { get; set; } = new();

}

