using JetBrains.Annotations;
using UnityEngine;
using DecentM.Collections;

using UdonSharp;
using VRC.SDKBase;

namespace DecentM.Video
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VideoSystem : UdonSharpBehaviour
    {
        private VideoEvents events;
        [SerializeField]
        private List/*<PlayerHandler>*/ playerHandlers;
        public AudioSource[] speakers;
        public ScreenHandler[] screens;

        public float volume = 1.0f;
        public float brightness = 1.0f;
        public int minFps = 1;
        public int maxFps = 144;

        private int currentPlayerHandlerIndex = 0;

        private void Start()
        {
            this.events = this.GetComponent<VideoEvents>();
            this.SendCustomEventDelayedSeconds(nameof(BroadcastInit), 0.1f);
        }

        internal void RegisterPlayerHandler(PlayerHandler behaviour)
        {
            this.playerHandlers.Add(behaviour);
        }

        private PlayerHandler GetCurrentPlayerHandler()
        {
            return (PlayerHandler)this.playerHandlers.ElementAt(this.currentPlayerHandlerIndex);
        }

        public void BroadcastInit()
        {
            this.DisableAllPlayers();
            this.EnablePlayer(0);
            this.SetVolume(this.volume);
            this.SetBrightness(this.brightness);
            this.PausePlayback();
            this.Seek(0);
            this.events.OnVideoPlayerInit();
        }

        [PublicAPI]
        public int ScreenCount
        {
            get { return this.screens.Length; }
        }

        [PublicAPI]
        public void RenderCurrentFrame(RenderTexture texture, int screenIndex)
        {
            if (this.screens == null || this.screens.Length == 0)
                return;
            if (screenIndex >= this.screens.Length || screenIndex < 0)
                screenIndex = 0;

            ScreenHandler screen = this.screens[screenIndex];
            screen.RenderToRenderTexture(texture);
        }

        [PublicAPI]
        public void RenderCurrentFrame(RenderTexture texture)
        {
            this.RenderCurrentFrame(texture, 0);
        }

        [PublicAPI]
        public Texture GetVideoTexture()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return null;

            return playerHandler.GetScreenTexture();
        }

        [PublicAPI]
        public void SetScreenTexture(Texture texture)
        {
            foreach (ScreenHandler screen in this.screens)
            {
                screen.SetTexture(texture);
            }
        }

        private void DisablePlayer(PlayerHandler player)
        {
            player.Unload();
            player.gameObject.SetActive(false);
        }

        private void DisablePlayer(int index)
        {
            PlayerHandler PlayerHandler = (PlayerHandler)this.playerHandlers.ElementAt(index);

            if (PlayerHandler == null)
                return;

            this.DisablePlayer(PlayerHandler);
        }

        private void EnablePlayer(PlayerHandler player)
        {
            player.gameObject.SetActive(true);
            player.Unload();
        }

        private void EnablePlayer(int index)
        {
            PlayerHandler playerHandler = (PlayerHandler)this.playerHandlers.ElementAt(index);

            if (playerHandler == null)
                return;

            this.EnablePlayer(playerHandler);
        }

        private void DisableAllPlayers()
        {
            foreach (PlayerHandler player in playerHandlers.ToArray())
            {
                this.DisablePlayer(player);
            }
        }

        [PublicAPI]
        public int NextPlayerHandler()
        {
            if (this.playerHandlers.Count == 0)
                return -1;

            int newIndex = this.currentPlayerHandlerIndex + 1;

            if (
                newIndex >= this.playerHandlers.Count
                || newIndex < 0
                || this.currentPlayerHandlerIndex == newIndex
                || this.playerHandlers.ElementAt(newIndex) == null
            )
            {
                newIndex = 0;
            }

            this.DisablePlayer(this.GetCurrentPlayerHandler());
            this.currentPlayerHandlerIndex = newIndex;

            PlayerHandler newPlayerHandler = this.GetCurrentPlayerHandler();

            if (newPlayerHandler == null)
                return -1;

            this.EnablePlayer(newPlayerHandler);
            this.events.OnPlayerSwitch(newPlayerHandler.type);

            return newIndex;
        }

        [PublicAPI]
        public bool IsPlaying()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return false;

            return playerHandler.IsPlaying();
        }

        [PublicAPI]
        public void StartPlayback()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            playerHandler.Play();
        }

        [PublicAPI]
        public void StartPlayback(float timestamp)
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            playerHandler.Play(timestamp);
        }

        [PublicAPI]
        public void Seek(float timestamp)
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            playerHandler.SetTime(timestamp);
            this.events.OnProgress(timestamp, this.GetDuration());
        }

        [PublicAPI]
        public void PausePlayback(float timestamp)
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            playerHandler.Pause();
            playerHandler.SetTime(timestamp);
            this.events.OnPlaybackStop(timestamp);
        }

        [PublicAPI]
        public void PausePlayback()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            playerHandler.Pause();
            this.events.OnPlaybackStop(playerHandler.GetTime());
        }

        private VRCUrl currentUrl;

        [PublicAPI]
        public void RequestVideo(VRCUrl url)
        {
            this.events.OnLoadRequested(url);
        }

        public void LoadVideo(VRCUrl url)
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            this.currentUrl = url;
            playerHandler.LoadURL(url);
        }

        [PublicAPI]
        public void Unload()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            this.currentUrl = null;
            playerHandler.Unload();
            this.events.OnUnload();
            this.Seek(0);
        }

        [PublicAPI]
        public VRCUrl GetCurrentUrl()
        {
            return this.currentUrl;
        }

        [PublicAPI]
        public float GetBrightness()
        {
            if (this.screens.Length == 0)
                return 1f;

            ScreenHandler screen = this.screens[0];
            return screen.GetBrightness();
        }

        [PublicAPI]
        public bool SetBrightness(float alpha)
        {
            if (alpha < 0 || alpha > 1)
                return false;

            foreach (ScreenHandler screen in this.screens)
            {
                screen.SetBrightness(alpha);
            }

            this.events.OnBrightnessChange(alpha);

            return true;
        }

        [PublicAPI]
        public bool SetVolume(float volume)
        {
            if (volume < 0 || volume > 1)
                return false;

            foreach (AudioSource speaker in this.speakers)
            {
                speaker.volume = volume;
            }

            this.volume = volume;
            this.events.OnVolumeChange(volume);

            return true;
        }

        [PublicAPI]
        public float GetVolume()
        {
            return this.volume;
        }

        [PublicAPI]
        public bool GetMuted()
        {
            return this.volume > 0;
        }

        [PublicAPI]
        public float GetDuration()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return 0;

            return playerHandler.GetDuration();
        }

        [PublicAPI]
        public float GetTime()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return 0;

            return playerHandler.GetTime();
        }
    }
}
