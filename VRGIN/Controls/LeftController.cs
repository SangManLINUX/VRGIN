using UnityEngine;

namespace VRGIN.Controls
{

    public class LeftController : Controller
    {
        public static LeftController Create()
        {
            var leftHand = new GameObject("Left Controller").AddComponent<LeftController>();
            leftHand.Tracking.inputSource = Valve.VR.SteamVR_Input_Sources.LeftHand;

            return leftHand;
        }
    }
}
