using System.Collections.Generic;

using UnityEditor;

using DecentM.Shared.Editor;
using DecentM.Video.Plugins;
using DecentM.VideoRatelimit;

namespace DecentM.Video.Editor
{
    public static class VideoPlayerAutoFixer
    {
        [MenuItem("DecentM/Video/Fix")]
        public static bool OnPerformFixes()
        {
            return true;
        }
    }
}
