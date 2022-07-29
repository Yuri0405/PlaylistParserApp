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

            bool isPlaylist = IsPlaylist(url);

            playlist.PlaylistName = _doc.DocumentNode.SelectSingleNode("//*[@id='page-container__first-linked-element']").InnerText;
            playlist.PlaylistName = playlist.PlaylistName.Trim();
            try
            {
                playlist.Description = _doc.DocumentNode.SelectSingleNode("//*[@class='truncated-content-container']/span/p").InnerText;
                playlist.Description = playlist.Description.Trim();
            }
            catch(NullReferenceException)
            {
                playlist.Description = null;
            }
            playlist.PlaylistPictureURL = GetRightUrl(_doc.DocumentNode.SelectSingleNode("//*[@class='product-lockup']/div/div/picture/source").Attributes["srcset"].Value);

            List<HtmlNode> songNames;
            List<HtmlNode> artistNames;
            List<HtmlNode> durations;
            HtmlNode author;

            songNames = _doc.DocumentNode.SelectNodes("//*[@class = \"songs-list-row__song-name\"]").ToList();
            
            List<Song> songs = new List<Song>(songNames.Count);

            if (isPlaylist)
            {
                artistNames = _doc.DocumentNode.SelectNodes("//*[@role = 'row']//div[@class = 'songs-list-row__by-line']/span").ToList();
                durations = _doc.DocumentNode.SelectNodes("//*[@role = 'row']/div[5]/div[1]/time").ToList();
                List<HtmlNode> albums = _doc.DocumentNode.SelectNodes("//*[@class = 'songs-list__col songs-list__col--album typography-body']").ToList();

                for (int i = 0; i < songNames.Count; i++)
                {
                    songs.Add(new Song
                    {
                        SongName = songNames[i].InnerText.Trim(),
                        Author = TrimInnerText(artistNames[i].InnerText),
                        AlbumName = albums[i].InnerText.Trim(),
                        Duration = durations[i].InnerText.Trim()
                    });
                }
            }
            else
            {
                author = _doc.DocumentNode.SelectSingleNode("//*[@class = 'product-creator typography-large-title']");
                durations = _doc.DocumentNode.SelectNodes("//*[@class = 'songs-list-row__length']").ToList();

                for (int i = 0; i < songNames.Count; i++)
                {
                    songs.Add(new Song
                    {
                        SongName = songNames[i].InnerText.Trim(),
                        Author = TrimInnerText(author.InnerText),
                        AlbumName = playlist.PlaylistName,
                        Duration = durations[i].InnerText.Trim()
                    });
                }
            }

            playlist.Songs = songs;
            return playlist;
        }

        private string TrimInnerText(string nodeInnerText)
        {
            string[] words = nodeInnerText.Split(new char[] { ' ', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return string.Join(" ", words);
        }
        //method for split array of image urls to get right one
        private string GetRightUrl(string entryStringOfUrls)
        {
            string result = "Not Found";
            string[] urls = entryStringOfUrls.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach(string url in urls)
            {
                if (url.EndsWith("400w") || url.EndsWith("600w"))
                {
                    result = url.Substring(0,url.Length-4);
                }
            }
            return result;
        }

        private bool IsPlaylist( string url)
        {
            int result = url.IndexOf("playlist");

            if(result != -1 && result != 0)
                return true;
            return false;
        }

        

    }
}
