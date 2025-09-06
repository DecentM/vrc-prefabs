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
        public static PluginRequirements GetRequirements(VideoUI ui)
        {
            PluginRequirements requirements = new PluginRequirements();

            requirements.system = ui.GetComponentInChildren<VideoSystem>();
            requirements.events = ui.GetComponentInChildren<VideoEvents>();

            return requirements;
        }
    }
}
