using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistParserApp.Models
{
    internal class Song
    {
        public string? SongName { get; set; }
        public string? Author { get; set; }
        public string? AlbumName { get; set; }
        public string? Duration { get; set; }
    }
}
