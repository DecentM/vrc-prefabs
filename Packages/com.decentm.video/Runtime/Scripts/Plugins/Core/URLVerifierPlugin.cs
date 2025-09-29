using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace DecentM.Video.Plugins
{
    /// <summary>
    /// Makes the video player deny loading URLs that fail validation
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None), AddComponentMenu("DecentM/Video/Plugins/URLVerifier")]
    internal sealed class URLVerifierPlugin : VideoPlugin
    {
        private bool ValidateUrl(string url)
        {
            // Some super vague heuristics to prevent some InvalidURL errors and keep the rate limit from being hit
            // by other video players in the world unnecessarily
            if (url == null || url == "")
                return false;

            if (
                url.StartsWith("http://")
                || url.StartsWith("https://")
                || url.StartsWith("localhost")
            )
                return true;

            if (url.Split('/').Length >= 2)
                return true;

            return false;
        }

        protected override void OnLoadRequested(VRCUrl url)
        {
            if (this.ValidateUrl(url.ToString()))
                return;

            this.system.DenyVideoRequest(url);
        }
    }
}
