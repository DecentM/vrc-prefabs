using JetBrains.Annotations;
using UnityEngine;
using DecentM.Collections;

using UdonSharp;
using VRC.SDKBase;
using System;

namespace DecentM.Video
{

    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VideoSystem : UdonSharpBehaviour
    {
        [NonSerialized]
        private VideoEvents events;

        [SerializeField]
        private List/*<PlayerHandler>*/ playerHandlers;

        [SerializeField]
        private AudioSource[] speakers;

        [SerializeField]
        private VideoScreen[] screens;

        public float volume = 1.0f;
        public float brightness = 1.0f;

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

            this.Pause();
            this.Seek(0);

#if UNITY_EDITOR
            if (this.GetCurrentPlayerHandler().type == nameof(VideoHandlerType.AVPro))
            {
                this.currentPlayerHandlerIndex = this.NextPlayerHandler();
            }
#endif

            this.events.OnVideoPlayerInit();
        }
        internal void ChangeScreenResolution(float width, float height)
        {
            float aspectRatio = width / height;

            for (int i = 0; i < this.screens.Length; i++)
            {
                VideoScreen screen = this.screens[i];

                if (screen == null)
                    continue;

                // Change physical screen size - should be a plugin
                /* screen.SetSize(
                    new Vector2(
                        screen.transform.localScale.x,
                        screen.transform.localScale.x / aspectRatio
                    )
                ); */

                screen.SetAspectRatio(aspectRatio);
                this.events.OnScreenResolutionChange(screen, width, height);
            }
        }


        [PublicAPI]
        public int ScreenCount
        {
            get { return this.screens.Length; }
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
            foreach (VideoScreen screen in this.screens)
            {
                screen.SetTexture(texture);
            }

            this.events.OnScreenTextureChange();
        }

        private void DisablePlayer(PlayerHandler player)
        {
            player.Unload();
            player.gameObject.SetActive(false);
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

#if UNITY_EDITOR
        [RecursiveMethod]
#endif
        internal int NextPlayerHandler()
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
            this.events.OnPlayerChange(newPlayerHandler.type);

            foreach (VideoScreen screen in this.screens)
            {
                screen.SetIsAVPro(newPlayerHandler.type == nameof(VideoHandlerType.AVPro));
            }

#if UNITY_EDITOR
            if (newPlayerHandler.type == nameof(VideoHandlerType.AVPro))
            {
                newIndex = this.NextPlayerHandler();
            }
#endif

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
        public void Play()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            playerHandler.Play();
        }

        [PublicAPI]
        public void Play(float timestamp)
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
        public void Pause(float timestamp)
        {
            this.Pause();
            this.Seek(timestamp);
        }

        [PublicAPI]
        public void Pause()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            playerHandler.Pause();
            this.events.OnPause(playerHandler.GetTime());
        }

        private VRCUrl currentUrl;

        [PublicAPI]
        public void RequestVideo(VRCUrl url)
        {
            this.events.OnLoadRequested(url);
        }

        internal void LoadVideo(VRCUrl url)
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            this.currentUrl = url;
            playerHandler.LoadURL(url);
        }

        [PublicAPI]
        public void Stop()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            this.currentUrl = null;
            playerHandler.Unload();
            this.events.OnStop();
            this.Seek(0);
        }

        [PublicAPI]
        public VRCUrl GetUrl()
        {
            return this.currentUrl;
        }

        [PublicAPI]
        public float GetBrightness()
        {
            if (this.screens.Length == 0)
                return 1f;

            VideoScreen screen = this.screens[0];
            return screen.GetBrightness();
        }

        [PublicAPI]
        public bool SetBrightness(float alpha)
        {
            if (alpha < 0 || alpha > 1)
                return false;

            foreach (VideoScreen screen in this.screens)
            {
                screen.SetBrightness(alpha);
            }

            this.events.OnBrightnessChange(alpha);

            return true;
        }

        [PublicAPI]
        public bool SetVolumeNoBroadcast(float volume)
        {
            if (volume < 0 || volume > 1)
                return false;

            foreach (AudioSource speaker in this.speakers)
            {
                speaker.volume = volume;
            }

            this.volume = volume;

            return true;
        }

        [PublicAPI]
        public bool SetVolume(float volume)
        {
            bool result = this.SetVolumeNoBroadcast(volume);

            this.events.OnVolumeChange(volume);

            return result;
        }

        [PublicAPI]
        public float GetVolume()
        {
            return this.volume;
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
