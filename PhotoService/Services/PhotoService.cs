using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FeyenZylstra.PhotoKiosk.Options;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace FeyenZylstra.PhotoKiosk.Services
{
    public class PhotoService
    {
        private readonly PhotoOptions _options;
        private readonly Random _random;

        public PhotoService(IOptions<PhotoOptions> options)
        {
            _options = options.Value;
            _random = new Random();
        }

        public Task<Stream> GetPhotoAsync(string id)
        {
            var path = GetStoragePath();
            var filename = Path.Combine(path, id + ".png");

            return Task.FromResult<Stream>(File.OpenRead(filename));
        }

        public (Stream, string) GetRandomPhoto()
        {
            var path = GetStoragePath();
            var files = Directory.GetFiles(path, "*.png").ToArray();
            var index = _random.Next(0, files.Length);
            var filename = Path.GetFileName(files[index]);
            
            return (File.OpenRead(files[index]), filename);
        }

        public Task SavePhotoAsync(Stream stream, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var bannerImage = Image.Load(_options.LogoFilename);

                bannerImage.Mutate(x => x.Resize(_options.LogoWidth, _options.LogoHeight));

                var path = GetStoragePath();
                var count = Directory.EnumerateFiles(path, "*.png").Count();
                var filename = Path.Combine(path, $"{count:00000}.png");

                using (var image = Image.Load(stream))
                using (var fileStream = File.OpenWrite(filename))
                {
                    image.Mutate(x =>
                    {
                        x.DrawImage(bannerImage, PixelBlenderMode.Normal, 1.0f,
                            new Point(_options.LogoX, _options.LogoY));
                    });

                    image.SaveAsPng(fileStream);
                }
            }, cancellationToken);
        }

        private string GetStoragePath()
        {
            var path = Path.Combine(_options.StoragePath, DateTime.UtcNow.ToString("yyyy-MM-dd"));

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}
