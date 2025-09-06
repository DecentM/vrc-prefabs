using System.Collections.Generic;

using UnityEngine;
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
            Debug.Log("Autofixer running...");

            FixRatelimits();

            List<VideoUI> players =
                ComponentCollector<VideoUI>.CollectFromActiveScene();

            foreach (VideoUI player in players)
            {
                VideoPlugin[] plugins = player.GetComponentsInChildren<VideoPlugin>();
                PluginRequirements requirements = PluginManager.GetRequirements(player);

                foreach (VideoPlugin plugin in plugins)
                {
                    plugin.events = requirements.events;
                    plugin.system = requirements.system;
                    plugin.pubsubHosts = new Pubsub.PubsubHost[] { requirements.events };

                    Inspector.SaveModifications(plugin);
                }
            }

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
