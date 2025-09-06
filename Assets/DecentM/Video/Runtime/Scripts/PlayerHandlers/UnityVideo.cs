using UdonSharp;

namespace DecentM.Video.Handlers
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class UnityVideo : PlayerHandler
    {
        public override VideoHandlerType type => VideoHandlerType.Unity;
    }
}
