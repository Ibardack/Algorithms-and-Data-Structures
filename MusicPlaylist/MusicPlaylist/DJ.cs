using System;
using System.Collections.Generic;
using System.Text;

namespace MusicPlaylist
{
    public class DJ //extra feature added to main file
    {
        private List<Song> _playlist;
        private Dictionary<string, int> _songSkipCount;
        private Random _random;
        private bool _isSmartQueueEnabled;

        public DJ(List<Song> playlist)
        {
            _playlist = playlist ?? throw new ArgumentNullException(nameof(playlist));
            _songSkipCount = new Dictionary<string, int>();
            _random = new Random();
            _isSmartQueueEnabled = false; //default state
        }

        public void ToggleSmartQueue(bool isEnabled)
        {
            _isSmartQueueEnabled = isEnabled;
            Console.WriteLine(isEnabled ? "Smart Queue is now enabled. " : "Smart Queue is now disabled");

        }
        public Song GetNextSong(Song currentSong)
        {
            if (_isSmartQueueEnabled)
            {
                var nextSongs = GetPotentialNextSongs(currentSong);
                return PickSmartSong(nextSongs);
            }
            else
            {
                return _playlist[_random.Next(_playlist.Count)];
            }
        }

        private List<Song> GetPotentialNextSongs(Song currentSong)
        {
            return _playlist.Where(song => song != currentSong)
            .OrderBy(song => _songSkipCount.ContainsKey(song.Name) ? _songSkipCount[song.Name] : 0) // considers skips 
            .ThenBy(song => song.Genre)
            .ThenBy(song => song.Duration)
            .ToList();
        }

        private Song PickSmartSong(List<Song> songs)
        {
            var selectedSong = songs[_random.Next(_songSkipCount.Count)];

            if (!_songSkipCount.ContainsKey(selectedSong.Name))
            {
                _songSkipCount[selectedSong.Name] = 0;
            }
            return selectedSong;

        }

        public void RegisterSongSkip(Song song)
        {
            if (!_songSkipCount.ContainsKey(song.Name))

            {
                _songSkipCount[song.Name] = 0;
            }

            _songSkipCount[song.Name]++;

        }

        public void AddToPlaylist(Song song)
        {
            if (!_playlist.Contains(song))
            {
                _playlist.Add(song);
            }
        }

        public void RemoveSongFromPlaylist(Song song)
        {
            if (_playlist.Contains(song))
            {
                _playlist.Remove(song);
            }
        }

        public void ResetSkipCount()
        {
            _songSkipCount.Clear();
        }

    }
}