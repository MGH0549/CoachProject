
namespace Coaching.Tests;

public sealed partial class TestPhotos
{
    public string SignedUrl(string reference) => "https://photos.example.test/" + reference + "?expires=300";
}
