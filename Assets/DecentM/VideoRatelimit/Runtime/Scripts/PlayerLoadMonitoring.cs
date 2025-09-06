using UdonSharp;

namespace DecentM.VideoRatelimit
{
    public class PlayerLoadMonitoring : UdonSharpBehaviour
    {
        public VideoRatelimitSystem system;

        public override void OnVideoReady()
        {
            this.system.OnPlayerLoad();
        }
    }
}
