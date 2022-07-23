using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using PlaylistParserApp.Models;
using PlaylistParserApp.Services;

namespace PlaylistParserApp.ViewModels
{
    internal class MainWindowViewModel:INotifyPropertyChanged
    {
        private string? _somestaff;
        private string? _enteredURL;
        private ImageLoader? _imageLoader;
        private Bitmap? _playlistCover;
        private readonly HtmlPlaylistParser _playlistParser;

        public Playlist PlayList { get; set; }

        public Bitmap? PlaylistCover { get { return _playlistCover; } 
            set {
                _playlistCover = value;
                OnPropertyChanged("PlaylistCover");
            } }
        public string ParsedPlaylist => "Wait for playlist parsing";
        public string SomeStaff { get { return _somestaff; }
            set 
            {
                _somestaff = value;
                OnPropertyChanged("Playlist");
            } }
        public string EnteredURL
        {
            get { return _enteredURL; }
            set
            {
                _enteredURL = value;
                OnPropertyChanged("EnteredURL");
            }
        }

        public MainWindowViewModel()
        {
            _playlistParser = new HtmlPlaylistParser();
        }
        public async void ButtonParsePlaylist()
        {
            PlayList = _playlistParser.ParsePlayList(EnteredURL);
            _imageLoader = new ImageLoader(PlayList.PlaylistPictureURL);
            PlaylistCover = await _imageLoader.LoadCover();
            OnPropertyChanged("PlayList");           
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
