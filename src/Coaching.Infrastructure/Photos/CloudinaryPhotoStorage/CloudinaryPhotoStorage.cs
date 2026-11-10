using Coaching.Application;
using Microsoft.Extensions.Options;

namespace Coaching.Infrastructure.Photos;

public sealed partial class CloudinaryPhotoStorage(HttpClient http, IOptions<CloudinaryOptions> options, ICairoTime time) : IPhotoStorage
{
    public const int MaxBytes = 8 * 1024 * 1024;
}
