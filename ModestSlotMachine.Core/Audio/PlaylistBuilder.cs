using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModestSlotMachine.Core.Audio
{
    public class PlaylistBuilder
    {
        private List<Track> _tracks = new List<Track>();
        private Guid? _firstTrackId;

        public static PlaylistBuilder Create()
        {
            return new PlaylistBuilder();
        }

        public PlaylistBuilder Next(Track track)
        {
            _tracks.Add(track);
            return this;
        }

        public PlaylistBuilder StartWith(Guid? id)
        {
            if (id.HasValue && Guid.Empty != id.Value)
                _firstTrackId = id;
            return this;
        }

        public Playlist Build()
        {
            if (_firstTrackId == null)
            {
                _firstTrackId = _tracks.First().Id;
            }

            Track firstTrack = _tracks.First(t => t.Id == _firstTrackId);
            int firstTrackIndex = _tracks.IndexOf(firstTrack);

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
