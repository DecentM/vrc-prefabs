using UdonSharp;

namespace DecentM.Video.Handlers
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AVProVideo : PlayerHandler
    {
        public override string type => nameof(VideoHandlerType.AVPro);
    }
}
