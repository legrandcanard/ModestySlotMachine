using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestySlotMachine.Core.Audio
{
    public class PlaylistBuilder
    {
        private List<Track> _tracks = new List<Track>();
        private int _firstIndex;

        public static PlaylistBuilder Create()
        {
            return new PlaylistBuilder();
        }

        public PlaylistBuilder Next(Track track)
        {
            _tracks.Add(track);
            return this;
        }

        public PlaylistBuilder StartWith(int index)
        {
            _firstIndex = index;
            return this;
        }

        public Playlist Build()
        {
            int firstTrackIndex = _firstIndex;

            var firstPart = _tracks.Skip(firstTrackIndex).Take(_tracks.Count - firstTrackIndex).ToArray();
            var secondPart = _tracks.Take(firstTrackIndex).ToArray();

            var playlist = new Playlist();

            foreach (var item in firstPart)
                playlist.Tracks.Add(item);

            foreach (var item in secondPart)
                playlist.Tracks.Add(item);

            return playlist;
        }
    }
}
