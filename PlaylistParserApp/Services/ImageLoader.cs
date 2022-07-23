using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistParserApp.Services
{
    internal class ImageLoader
    {
        private HttpClient _stream_client;
        private readonly string _url;

        public ImageLoader(string url)
        {
            _stream_client = new HttpClient();
            _url = url;
        }

        private async Task<Stream> LoadCoverBitmapAsync()
        {
            var data = await _stream_client.GetByteArrayAsync(_url);
            return new MemoryStream(data);
        }
        
        public async Task<Bitmap> LoadCover()
        {
            Bitmap bitmap;
            await using(var imageStream = await LoadCoverBitmapAsync())
            {
                bitmap = await Task.Run(() => Bitmap.DecodeToWidth(imageStream,200)); 
            }
            return bitmap;
        }
        
    }
}
