using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlaylist
{
    internal class SongNode
    {
        public string Title;
        public string Artist;
        public string Genre;
        public string Duration;
        public string Album;

        public SongNode Prev;
        public SongNode Next;

        public SongNode(string title, string artist, string genre, string duration, string album)
        {
            Title = title;
            Artist = artist;
            Genre = genre;
            Duration = duration;
            Album = album;
           
        }
    }
}
