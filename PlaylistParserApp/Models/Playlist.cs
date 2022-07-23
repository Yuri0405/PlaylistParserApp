using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistParserApp.Models
{
    internal class Playlist
    {
        public string? PlaylistName { get; set; }
        public string? Description { get; set; }
        public string? PlaylistPictureURL { get; set; }
        public List<Song>? Songs { get; set; }

    }
}
