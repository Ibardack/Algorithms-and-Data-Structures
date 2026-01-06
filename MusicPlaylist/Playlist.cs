using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Xml;

namespace MusicPlaylist
{
    internal class Playlist
    {
        public SongNode head;
        public SongNode tail;
        public SongNode current;

        public Random rand = new Random();
        private SongNode newNode;

        public void AddSong(string title, string artist, string album, string duration, string genre)
        {
            SongNode newNode = new SongNode(title, artist, album, duration, genre);

            if (head == null)
            {
                head = tail = current = newNode;
            }
            else
            {
                tail.Next = newNode;
                newNode.Prev = tail;
                tail = newNode;
            }

        }
        public bool DeleteSongByTitle(string title)
        {
            SongNode temp = head;

            while (temp != null)
            {
                if (temp.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    RemoveNode(temp);
                    return true;
                }
                temp = temp.Next;
            }
            return false;
        }
        public bool DeleteSongByIndex(int index)
        {
            int count = 0;
            SongNode temp = head;

            while (temp != null)
            {
                if (count == index)
                {
                    RemoveNode(temp);
                    return true;
                }
                count++;
                temp = temp.Next;
            }
            return false;
        }
        private void RemoveNode(SongNode node)
        {
            if (node.Prev != null) node.Prev.Next = node.Next;
            else head = node.Next;

            if (node.Next != null) node.Next.Prev = node.Prev;
            else tail = node.Prev;
        }
        public int SearchSong(string title)
        {
            int index = 0;
            SongNode temp = head;

            while (temp != null)
            {
                if (temp.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                    return index;

                temp = temp.Next;
                index++;
            }
            return -1;
        }
        private List<SongNode> ToList() //Convertng to list for better sorting
        {
            List<SongNode> list = new List<SongNode>();
            SongNode temp = head;

            while (temp != null)
            {
                list.Add(temp);
                temp = temp.Next;
            }
            return list;
        }
        public void DisplaySortedByArtists()
        {
            var list = ToList();
            list.Sort((a, b) => a.Artist.CompareTo(b.Artist));

            Console.WriteLine("\n--- Song Sorted by Artist ---");
            foreach (var s in list)
                PrintSong(s);
        }
        public void DisplaySortedByDuration()
        {
            var list = ToList();
            list.Sort((a, b) => TimeSpan.Parse(a.Duration).CompareTo(TimeSpan.Parse(b.Duration)));

            Console.WriteLine("\n--- Song Sorted by Duration ---");
            foreach (var s in list)
                PrintSong(s);
        }
        public void Shuffle()

        {
            List<SongNode> list = ToList();
            int n = list.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
            head = list[0];
            head.Prev = null;

            for (int i = 1; i < n; i++)
            {
                list [i - 1].Next = list[i];
                list [i].Prev = list[i - 1];
            }
            tail = list[n - 1];
            tail.Next = null;

            current = head;

        }
        public void Playback()
        {
            if (current?.Next != null)
                current = current.Next;

            Console.WriteLine("\nPlaying: + current.Title");
        }
        public void PlayPrev()
        {
            if (current?.Prev != null)
                current = current.Prev;

            Console.WriteLine("\nPlaying: + current.Title");
        }

        public void PlaySong(string title)
        {
            SongNode temp = head;

            while (temp != null)
            {
                if (temp.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    current = temp;
                    Console.WriteLine("\nPlaying: + current.Title");
                    return;
                }
                temp = temp.Next;
            }
            Console.WriteLine("Song not found. ");
        }
        public void PlayLoop(int loops)
        {
            if (current == null) return;

            Console.WriteLine($"]nLooping \" {current.Title}\" {loops} times:");
            for (int i = 0; i < loops; i++)
                Console.WriteLine($"loop {i}: {current.Title}");
        }
        public void Display()
        {
            Console.WriteLine("\n--- Playlist ---");
            SongNode temp = head;

            while (temp != null)
            {
                PrintSong(temp);
                temp = temp.Next;
            }
        }
        private void PrintSong(SongNode s)
        {
            Console.WriteLine($"{s.Title} | {s.Album} | {s.Duration} | {s.Genre}");
        }
    }
}
