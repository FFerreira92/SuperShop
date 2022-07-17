using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace SuperShop.Helpers
{
    public interface IBlobHelper
    {
        Task<Guid> UploadBlobAsync(IFormFile file, String containerName);

        Task<Guid> UploadBlobAsync(byte[] file, String containerName);

        Task<Guid> UploadBlobAsync(string image, String containerName);
    }
}
