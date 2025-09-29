using DecentM.Collections.Generics;
using JetBrains.Annotations;
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace DecentM.Video
{

    [
        UdonBehaviourSyncMode(BehaviourSyncMode.None),
        AddComponentMenu("DecentM/Video/VideoSystem"),
        RequireComponent(typeof(VideoEvents), typeof(GameObjectList))
    ]
    public sealed class VideoSystem : UdonSharpBehaviour
    {
        [NonSerialized] private GameObjectList playerHandlers;

        [SerializeField] private AudioSource[] speakers;
        [SerializeField] private VideoScreen[] screens;

        [NonSerialized] private VideoEvents events;

        private int currentPlayerHandlerIndex = 0;

        private void Start()
        {
            this.playerHandlers = this.GetComponent<GameObjectList>();
            this.events = this.GetComponent<VideoEvents>();
            // In case there's no plugin to set up ownership, we assume the local player is the owner
            this.ownerId = VRCPlayerApi.GetPlayerId(Networking.LocalPlayer);
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

#if UNITY_EDITOR
            // Skip AVPro when running in editor
            if (this.GetCurrentPlayerHandler().type == nameof(VideoHandlerType.AVPro))
            {
                this.currentPlayerHandlerIndex = this.NextPlayerHandler();
            }
#endif

            this.events.OnVideoPlayerInit();
        }

        /// <summary>
        /// Calculates the aspect ratio, and sets it on supported screens. The default prefab comes with a supported screen.
        /// </summary>
        [PublicAPI]
        public bool ChangeScreenResolution(float width, float height)
        {
            float aspectRatio = width / height;
            int successes = 0;

            for (int i = 0; i < this.screens.Length; i++)
            {
                VideoScreen screen = this.screens[i];

                if (!screen.SetAspectRatio(aspectRatio))
                    continue;

                successes++;
            }

            this.events.OnScreenResolutionChange(width, height);
            return successes == this.screens.Length;
        }

        /// <summary>
        /// Returns a reference to the screen texture. This shouln't be called every frame, but instead in reaction to state changes, such as when the player starts playing.
        /// </summary>
        [PublicAPI]
        public Texture GetScreenTexture()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return null;

            return playerHandler.GetScreenTexture();
        }

        /// <summary>
        /// Sets the screen texture. This shouln't be called every frame, but instead in reaction to state changes, such as when the player starts playing.
        /// The default prefab comes with a plugin that takes care of this.
        /// </summary>
        [PublicAPI]
        public bool SetScreenTexture(Texture texture)
        {
            int successes = 0;

            foreach (VideoScreen screen in this.screens)
            {
                if (!screen.SetTexture(texture))
                    continue;

                successes++;
            }

            this.events.OnScreenTextureChange();
            return successes == this.screens.Length;
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
            foreach (PlayerHandler player in this.playerHandlers.ToArray())
            {
                this.DisablePlayer(player);
            }
        }

#if UNITY_EDITOR
        // Skip AVPro when running in editor
        [RecursiveMethod]
#endif
        internal int NextPlayerHandler()
        {
            if (this.playerHandlers.Count() == 0)
                return -1;

            int newIndex = this.currentPlayerHandlerIndex + 1;

            if (
                newIndex >= this.playerHandlers.Count()
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

#if UNITY_EDITOR
            // Skip AVPro when running in editor
            if (newPlayerHandler.type == nameof(VideoHandlerType.AVPro))
            {
                newIndex = this.NextPlayerHandler();
            }
#endif

            return newIndex;
        }

        /// <summary>
        /// Returns true if the player is currently playing
        /// </summary>
        [PublicAPI]
        public bool IsPlaying()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return false;

            return playerHandler.IsPlaying();
        }

        private bool UpdateAvProType(bool isAVPro)
        {
            int successes = 0;

            foreach (VideoScreen screen in this.screens)
            {
                if (!screen.SetIsAVPro(isAVPro))
                    continue;

                successes++;
            }

            return successes == this.screens.Length;
        }

        /// <summary>
        /// Starts playback from the current position
        /// </summary>
        [PublicAPI]
        public void Play()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            this.UpdateAvProType(playerHandler.type == nameof(VideoHandlerType.AVPro));
            playerHandler.Play();
        }

        /// <summary>
        /// Starts playback from the given timestamp
        /// </summary>
        [PublicAPI]
        public void Play(float timestamp)
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            this.UpdateAvProType(playerHandler.type == nameof(VideoHandlerType.AVPro));
            playerHandler.Play(timestamp);
        }

        /// <summary>
        /// Sets the position to the given timestamp, but does not start/stop playback
        /// </summary>
        [PublicAPI]
        public void Seek(float timestamp)
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            playerHandler.SetTime(timestamp);
            this.events.OnProgress(timestamp, this.GetDuration());
        }

        /// <summary>
        /// Pauses playback at the given timestamp
        /// </summary>
        [PublicAPI]
        public void Pause(float timestamp)
        {
            this.Pause();
            this.Seek(timestamp);
        }

        /// <summary>
        /// Pauses playback at the current position
        /// </summary>
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
        private VRCUrl approvalPending = VRCUrl.Empty;
        private float approvalTimeout = 0.3f;
        private int denials = 0;

        /// <summary>
        /// Requests playback of the given URL. If no plugin denies the request, loads the URL.
        /// </summary>
        [PublicAPI]
        public void RequestVideo(VRCUrl url)
        {
            if (!string.IsNullOrWhiteSpace(this.approvalPending.ToString()))
                return;

            this.denials = 0;
            this.approvalPending = url;
            this.SendCustomEventDelayedSeconds(nameof(CheckForDenials), this.approvalTimeout);
            this.events.OnLoadRequested(url);
        }

        public void CheckForDenials()
        {
            if (string.IsNullOrWhiteSpace(this.approvalPending.ToString())) return;

            if (this.denials == 0)
            {
                this.LoadVideo(this.approvalPending);
            }
            else
            {
                this.events.OnLoadError(VRC.SDK3.Components.Video.VideoError.InvalidURL);
                this.Stop();
                this.denials = 0;
            }

            this.approvalPending = VRCUrl.Empty;
        }

        [PublicAPI]
        public void DenyVideoRequest(VRCUrl url)
        {
            if (string.IsNullOrWhiteSpace(this.approvalPending.ToString()) || url != this.approvalPending)
                return;

            this.denials++;
        }

        internal void LoadVideo(VRCUrl url)
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            this.currentUrl = url;
            playerHandler.LoadURL(url);
        }

        /// <summary>
        /// Stops playback and unloads the current video
        /// </summary>
        [PublicAPI]
        public void Stop()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return;

            this.currentUrl = null;
            this.UpdateAvProType(false);
            playerHandler.Unload();
            this.events.OnStop();
        }

        /// <summary>
        /// Returns the currently loaded VRCUrl
        /// </summary>
        [PublicAPI]
        public VRCUrl GetUrl()
        {
            return this.currentUrl;
        }

        [SerializeField] private float baseGamma = 2.2f;
        [SerializeField] private float gammaRange = 0.4f;

        private float gamma = 2.2f;

        /// <summary>
        /// Returns the currently set gamma value, a float between 0 and 1
        /// </summary>
        [PublicAPI]
        public float GetGamma()
        {
            return (this.gamma - this.baseGamma) / this.gammaRange;
        }

        /// <summary>
        /// Sets the gamma value to the given value, which must be a normal value between 0 and 1
        /// </summary>
        public void SetGamma(float gamma)
        {
            this.gamma = this.baseGamma + gamma * this.gammaRange;
            this.SetBrightness(this.GetBrightness());
        }

        /// <summary>
        /// Returns the current brightness value, which is a float between 0 and 1.<br />
        /// Returns -1f if there are no screens, or if a screen is missing a renderer.<br />
        /// </summary>
        [PublicAPI]
        public float GetBrightness()
        {
            if (this.screens.Length == 0)
                return -1f;

            VideoScreen screen = this.screens[0];
            float screenBrightness = screen.GetBrightness();

            if (screenBrightness < 0f)
                return -1;

            return (float)Math.Pow(screen.GetBrightness(), 1 / this.gamma);
        }

        /// <summary>
        /// Returns the current brightness value (without sending events), which is a float between 0 and 1.<br />
        /// Returns -1f if there are no screens, or if a screen is missing a renderer.
        /// </summary>
        [PublicAPI]
        public bool SetBrightnessNoBroadcast(float alpha)
        {
            if (alpha < 0 || alpha > 1)
                return false;

            float linearBrightness = (float)Math.Pow(alpha, this.gamma);

            foreach (VideoScreen screen in this.screens)
            {
                screen.SetBrightness(linearBrightness);
            }

            return true;
        }

        /// <summary>
        /// Sets the brightness value to the given value, which must be a normal value between 0 and 1.<br />
        /// Returns false if it failed.
        /// </summary>
        [PublicAPI]
        public bool SetBrightness(float alpha)
        {
            if (!this.SetBrightnessNoBroadcast(alpha))
                return false;

            this.events.OnBrightnessChange(alpha);

            return true;
        }

        /// <summary>
        /// Sets the brightness value to the given value (without sending events), which must be a normal value between 0 and 1.<br />
        /// Returns false if it failed.
        /// </summary>
        [PublicAPI]
        public bool SetVolumeNoBroadcast(float volume)
        {
            if (volume < 0 || volume > 1)
                return false;

            float linearVolume = volume == 0 ? 0 : (float)Math.Pow(10, volume * 2 - 1) / 10;

            foreach (AudioSource speaker in this.speakers)
            {
                speaker.volume = linearVolume;
            }

            return true;
        }

        /// <summary>
        /// Sets the volume to the given value, which must be a normal value between 0 and 1.<br />
        /// Returns false if it failed.
        /// </summary>
        [PublicAPI]
        public bool SetVolume(float volume)
        {
            if (!this.SetVolumeNoBroadcast(volume))
                return false;

            this.events.OnVolumeChange(volume);

            return true;
        }

        /// <summary>
        /// Returns the current volume, as a float between 0 and 1.<br />
        /// </summary>
        [PublicAPI]
        public float GetVolume()
        {
            float rawVolume = this.speakers.Length > 0 ? this.speakers[0].volume : 0;

            if (rawVolume == 0)
                return 0;

            return (float)(Math.Log10(rawVolume * 10) + 1) / 2;
        }

        /// <summary>
        /// Returns the duration of the currently loaded VRCUrl, or -1 if there's no video loaded
        /// </summary>
        [PublicAPI]
        public float GetDuration()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return -1;

            return playerHandler.GetDuration();
        }

        /// <summary>
        /// Returns the current position within the loaded video. Use "time / duration" to get the progress between 0 and 1.
        /// </summary>
        [PublicAPI]
        public float GetTime()
        {
            PlayerHandler playerHandler = this.GetCurrentPlayerHandler();

            if (playerHandler == null)
                return 0;

            return playerHandler.GetTime();
        }

        private int ownerId = -1;

        public void SetOwner(VRCPlayerApi player)
        {
            this.ownerId = VRCPlayerApi.GetPlayerId(player);
        }

        public VRCPlayerApi GetOwner()
        {
            return VRCPlayerApi.GetPlayerById(this.ownerId);
        }

        public bool IsLocallyOwned()
        {
            return this.ownerId == VRCPlayerApi.GetPlayerId(Networking.LocalPlayer);
        }
    }
}