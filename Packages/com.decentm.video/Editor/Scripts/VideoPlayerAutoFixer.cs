using System.Collections.Generic;

using UnityEditor;

using DecentM.Shared.Editor;
using DecentM.Video.Plugins;
using DecentM.VideoRatelimit;

namespace DecentM.Video.Editor
{
    public static class VideoPlayerAutoFixer
    {
        [MenuItem("DecentM/VideoPlayer/Run Autofixer")]
        public static bool OnPerformFixes()
        {
            FixRatelimits();

            return true;
        }

        private static void FixRatelimits()
        {
            List<VideoRatelimitSystem> ratelimits =
                ComponentCollector<VideoRatelimitSystem>.CollectFromActiveScene();

            if (ratelimits.Count <= 0)
                return;

            VideoRatelimitSystem ratelimit = ratelimits[0];

            if (ratelimit == null)
                return;

            List<LoadRequestHandlerPlugin> loadHandlers =
                ComponentCollector<LoadRequestHandlerPlugin>.CollectFromActiveScene();

            foreach (LoadRequestHandlerPlugin loadHandler in loadHandlers)
            {
                loadHandler.ratelimit = ratelimit;

                Inspector.SaveModifications(loadHandler);
            }
        }
    }
}
