using UdonSharp;

namespace DecentM.Video.Handlers
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class UnityVideo : PlayerHandler
    {
        public override string type => nameof(VideoHandlerType.Unity);
    }
}
