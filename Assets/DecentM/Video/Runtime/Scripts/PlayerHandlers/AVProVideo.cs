using UdonSharp;

namespace DecentM.Video.Handlers
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class AVProVideo : PlayerHandler
    {
        public override VideoHandlerType type => VideoHandlerType.AVPro;
    }
}
