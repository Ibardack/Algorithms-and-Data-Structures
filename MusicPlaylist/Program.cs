using MusicPlaylist;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.Design;



namespace MusicPlaylist
{
    public class Song
    {
        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public TimeSpan Duration { get; set; }
        public string Genre { get; set; }

        public Song(string name, string artist, string album, TimeSpan duration, string genre)
        {
            Name = name;
            Duration = duration;
            Artist = artist;
            Genre = genre;
            Album = album;
        }

        public override string ToString() => $"{Name} by {Artist} [{Album}] - {Genre} ({Duration})";  // testing method to display the information 
    }

     
    
}

namespace MusicPlaylist
{
}

class Program
{
    static void Main()
    {
        var playlist = new List<Song>
            {
                new Song("Shape of you", "Ed Sheeran", "Divide", TimeSpan.Parse("03:53:00"), "Pop"),
                new Song("Bohemian Rhapsody", "Queen", "A Night at the Opera", TimeSpan.Parse("05:55:00"), "Rock"),
                new Song("Blinding Lights", "The Weeknd", "After Hours", TimeSpan.Parse("03:20:00"), "Synth-Pop"),
                new Song("Rolling in the Deep", "Adele", "21", TimeSpan.Parse("03:48:00"), "Soul"),
                new Song("Hotel California", "Eagles", "Hotel California", TimeSpan.Parse("06:31:00"), "Rock"),
                new Song("Perfect", "Ed Sheeran", "Divide", TimeSpan.Parse("04:23:00"), "Pop"),
                new Song("Levitating", "Dua Lipa", "Future Nostalgia", TimeSpan.Parse("03:23:00"), "Disco-Pop"),
                new Song("Uptown Funk", "Mark Ronson ft. Bruno Mars", "Uptown Special", TimeSpan.Parse("04:30:00"), "Funk"),
                new Song("Counting Stars", "OneRepublic", "Native", TimeSpan.Parse("04:17:00"), "Pop-Rock"),
                new Song("Bad Guy", "Billie Eilish", "When We All Fall Asleep", TimeSpan.Parse("03:14:00"), "Electropop"),

            };

        var dj = new DJ(playlist);
        Playlist p = new Playlist();

        // Adding all songs to playlist

        foreach (var song in playlist)
        {
            p.AddSong(song.Name, song.Artist, song.Album, song.Duration.ToString(), song.Genre);
        }

        Console.WriteLine("===== Music player =====");
        bool keeprunning = true; //initially it was ending after optiuon 2 selection

        while (keeprunning)
        {

            Console.WriteLine("\nSelect an option: ");
            Console.WriteLine("1. DJ PLaylist (Smart Queue)");
            Console.WriteLine("2. Playlist Manager");
            Console.WriteLine("3. Exit");


            string choice = Console.ReadLine();
            if (choice == "1")
            {

                // logic for the dj 

                Console.WriteLine("Would you like to use Smart Queue? (yes/no)");
                string userInput = Console.ReadLine().ToLower();

                if (userInput == "yes")
                {
                    dj.ToggleSmartQueue(true);
                }
                else
                {
                    dj.ToggleSmartQueue(false);
                }

                dj.RegisterSongSkip(playlist[2]); // relates to order of songs (Blinding Lights is skipped)
                dj.RegisterSongSkip(playlist[6]);

                var nextSong = dj.GetNextSong(playlist[1]); // uses smart queue to get song
                Console.WriteLine("Next song: " + nextSong);

                var newSong = new Song("Next Song", "Artist X", "Album X", TimeSpan.Parse("03:00:00"), "Rock"); // add song to playlist and tests
                dj.AddToPlaylist(newSong);
                var nextSongAfterAdding = dj.GetNextSong(nextSong);
                Console.WriteLine("Next song after adding: " + nextSongAfterAdding); // reset skip count

                dj.ResetSkipCount();
                var nextSongAfterReset = dj.GetNextSong(nextSong);
                Console.WriteLine("Next song after reset: " + nextSongAfterReset);

            }

            else if (choice == "2")
            {
                bool inPlaylistManager = true;

                while (inPlaylistManager)
                {
                    Console.WriteLine("\n ===== Playlist Manager =====");
                    Console.WriteLine("1. Display Playlist");
                    Console.WriteLine("2. Add Song to Playlist");
                    Console.WriteLine("3. Remove Song from Playlist");
                    Console.WriteLine("4. Return to Main Page");

                    string playlistChoice = Console.ReadLine();

                    if (playlistChoice == "1")
                    {

                        Console.WriteLine("\nCurrent Playlist: ");
                        foreach (var song in playlist)
                        {
                            Console.WriteLine(song);
                        }

                    }
                    else if (playlistChoice == "2")
                    {

                        Console.WriteLine("Enter song name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter artist: ");
                        string artist = Console.ReadLine();
                        Console.WriteLine("Enter album: ");
                        string album = Console.ReadLine();
                        Console.WriteLine("Enter duration (hh:mm:ss): )");
                        TimeSpan duration = TimeSpan.Parse(Console.ReadLine());
                        Console.WriteLine("Enter genre: ");
                        string genre = Console.ReadLine();

                        var newSong = new Song(name, artist, album, duration, genre);
                        dj.AddToPlaylist(newSong);
                        Console.WriteLine($"Song '{newSong.Name}' added to playlist.");

                    }

                    else if (playlistChoice == "3")
                    {
                        Console.WriteLine("What is the name of the song you would like to remove? ");
                        string songNameToRemove = Console.ReadLine();

                        var songToRemove = playlist.FirstOrDefault(song => song.Name.Equals(songNameToRemove, StringComparison.OrdinalIgnoreCase));
                        if (songToRemove != null)
                        {

                            dj.RemoveSongFromPlaylist(songToRemove);
                            Console.WriteLine($"Song '{songToRemove.Name}' removed from playlist.");
                        }
                        else
                        {
                            Console.WriteLine("Song not found in playlist.");

                        }
                    }

                    else if (playlistChoice == "4")
                    {
                        inPlaylistManager = false;
                    }
                    else
                    {

                        Console.WriteLine("Invalid choice. Please try again.");
                    }
                }
            }
            else if (choice == "3")
            {

                keeprunning = false;
                Console.WriteLine("Exiting...");

            }
            else
            {
                Console.WriteLine("Invalid choice. Please try again.");
            }
        }
    }
}