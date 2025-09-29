using UdonSharp;
using UnityEngine;
using VRC.SDK3.Video.Components;

namespace DecentM.Video.Handlers
{
    [
        UdonBehaviourSyncMode(BehaviourSyncMode.None),
        AddComponentMenu("DecentM/Video/Core/UnityVideo"),
        RequireComponent(typeof(VRCUnityVideoPlayer))
    ]
    internal sealed class UnityVideo : PlayerHandler
    {
        public override string type => nameof(VideoHandlerType.Unity);
    }
}
