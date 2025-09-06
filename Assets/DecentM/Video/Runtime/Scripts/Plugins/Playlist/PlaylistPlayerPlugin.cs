using VRC.SDKBase;

namespace DecentM.Video.Plugins
{
    public class PlaylistPlayerPlugin : VideoPlugin
    {
        public VideoPlaylist playlist;

        protected override void OnVideoPlayerInit()
        {
            if (this.playlist == null)
                return;

            object[] next = this.playlist.GetCurrent();
            this.PlayItem(next);
        }

        protected override void OnPlaybackEnd()
        {
            if (this.playlist == null)
                return;

            object[] next = this.playlist.Next();
            this.PlayItem(next);
        }

        protected override void OnAutoRetryAbort()
        {
            if (this.playlist == null)
                return;

            object[] next = this.playlist.Next();
            this.PlayItem(next);
        }

        public void SetCurrentPlaylist(VideoPlaylist playlist)
        {
            this.playlist = playlist;
        }

        public void PlayItem(object[] item)
        {
            if (item == null)
                return;

            VRCUrl url = (VRCUrl)item[0];

            this.system.RequestVideo(url);
        }
    }
}
