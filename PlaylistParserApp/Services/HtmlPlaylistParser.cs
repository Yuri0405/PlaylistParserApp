using HtmlAgilityPack;
using PlaylistParserApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaylistParserApp.Services
{
    internal class HtmlPlaylistParser
    {
        private readonly HtmlWeb _web;
        private HtmlDocument? _doc;

        public HtmlPlaylistParser()
        {
            _web = new HtmlWeb();
        }

        public Playlist ParsePlayList(string url)
        {
            _doc = _web.Load(url);
            Playlist playlist = new Playlist();
            playlist.PlaylistName = _doc.DocumentNode.SelectSingleNode("//*[@id=\"playlistsDetails\"]/div/article[1]/div/section[2]/h2").InnerText;
            playlist.Description = _doc.DocumentNode.SelectSingleNode("//*[@id=\"playlistsDetails\"]/div/article[1]/div/section[2]/div[1]/span[1]").InnerText;
            playlist.Description = playlist.Description.Trim();
            playlist.PlaylistPictureURL = _doc.DocumentNode.SelectSingleNode("//*[@id=\"playlistsDetails\"]/div/article[1]/div/section[1]/img").Attributes["src"].Value;

            var songNames = _doc.DocumentNode.SelectNodes("//*[@id=\"playlistsDetails\"]/div/article[2]/ol/li/div[2]/div[1]/a").ToList();
            var artistNames = _doc.DocumentNode.SelectNodes("//*[@id=\"playlistsDetails\"]/div/article[2]/ol/li/div[2]/a").ToList();
            var durations = _doc.DocumentNode.SelectNodes("//*[@id=\"playlistsDetails\"]/div/article[2]/ol/li/time").ToList();

            int count = songNames.Count;
            List<Song> songs = new List<Song>();

            for (int i = 0; i < count; i++)
            {
                songs.Add(new Song
                {
                    SongName = songNames[i].InnerText,
                    Author = artistNames[i].InnerText,
                    Duration = durations[i].InnerText
                });
            }
            playlist.Songs = songs;
            return playlist;
        }

    }
}
