using UnityEngine;

using UnityEngine.UI;
using VRC.SDK3.Components;
using UdonSharp;
using VRC.SDKBase;

namespace DecentM.Video.Plugins
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PreloadURLPlugin : VideoPlugin
    {
        [SerializeField] private VRCUrl url;

        protected override void OnVideoPlayerInit()
        {
            this.system.RequestVideo(this.url);
            this.system.SetVolume(0);
        }
    }
}
