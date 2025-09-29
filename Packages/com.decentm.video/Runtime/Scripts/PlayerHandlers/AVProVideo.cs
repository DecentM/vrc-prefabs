using UdonSharp;
using UnityEngine;
using VRC.SDK3.Video.Components.AVPro;

namespace DecentM.Video.Handlers
{
    [
        UdonBehaviourSyncMode(BehaviourSyncMode.None),
        AddComponentMenu("DecentM/Video/Core/AVProVideo"),
        RequireComponent(typeof(VRCAVProVideoPlayer), typeof(VRCAVProVideoScreen))
    ]
    internal sealed class AVProVideo : PlayerHandler
    {
        public override string type => nameof(VideoHandlerType.AVPro);
    }
}
