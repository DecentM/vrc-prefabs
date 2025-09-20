using UdonSharp;

namespace DecentM.Video.Handlers
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    internal class UnityVideo : PlayerHandler
    {
        public override string type => nameof(VideoHandlerType.Unity);
    }
}
