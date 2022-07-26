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
            playlist.PlaylistName = _doc.DocumentNode.SelectSingleNode("//*[@id='page-container__first-linked-element']").InnerText;
            playlist.Description = _doc.DocumentNode.SelectSingleNode("//*[@class='truncated-content-container']/span/p").InnerText;
            playlist.Description = playlist.Description.Trim();
            playlist.PlaylistPictureURL = GetRightUrl(_doc.DocumentNode.SelectSingleNode("//*[@class='product-lockup']/div/div/picture/source").Attributes["srcset"].Value);
            

            var songNames = _doc.DocumentNode.SelectNodes("//*[@class = \"songs-list-row__song-name\"]").ToList();
            var artistNames = _doc.DocumentNode.SelectNodes("//*[@role = 'row']//div[@class = 'songs-list-row__by-line']/span").ToList();
            var durations = _doc.DocumentNode.SelectNodes("//*[@role = 'row']/div[5]/div[1]/time").ToList();

            int count = songNames.Count;
            List<Song> songs = new List<Song>();

            for (int i = 0; i < count; i++)
            {
                songs.Add(new Song
                {
                    SongName = songNames[i].InnerText,
                    Author = TrimInnerText(artistNames[i].InnerText),
                    Duration = durations[i].InnerText
                });
            }
            playlist.Songs = songs;
            return playlist;
        }

        private string TrimInnerText(string nodeInnerText)
        {
            string[] words = nodeInnerText.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", words);
        }

        private string GetRightUrl(string entryStringOfUrls)
        {
            string result = "Not Found";
            string targetEnding = "400w";
            string[] urls = entryStringOfUrls.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach(string url in urls)
            {
                if (url.EndsWith(targetEnding))
                {
                    result = url.Substring(0,url.Length-targetEnding.Length);
                }
            }
            return result;
        }

    }
}
