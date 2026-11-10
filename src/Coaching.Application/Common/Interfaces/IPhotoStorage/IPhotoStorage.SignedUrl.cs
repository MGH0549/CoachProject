
namespace Coaching.Application;

public partial interface IPhotoStorage
{
    string SignedUrl(string reference);
}
