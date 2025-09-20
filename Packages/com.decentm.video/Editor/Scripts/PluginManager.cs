using UnityEngine.Rendering.VirtualTexturing;

namespace DecentM.Video.Editor
{
    public struct PluginRequirements
    {
        public PluginRequirements(VideoEvents events, VideoSystem system)
        {
            this.events = events;
            this.system = system;
        }

        public VideoEvents events;
        public VideoSystem system;
    }

    public static class PluginManager
    {
        public static PluginRequirements GetRequirements(VideoSystem system)
        {
            PluginRequirements requirements = new PluginRequirements();

            requirements.system = system.GetComponentInChildren<VideoSystem>();
            requirements.events = system.GetComponentInChildren<VideoEvents>();

            return requirements;
        }
    }
}
