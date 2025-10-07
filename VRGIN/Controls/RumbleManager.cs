using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR;
using VRGIN.Core;
using VRGIN.Helpers;

namespace VRGIN.Controls
{
    /// <summary>
    /// Takes care of the rumble functionality.
    /// </summary>
    public class RumbleManager : ProtectedBehaviour
    {
        const float MILLI_TO_SECONDS = 1f / 1000f;
        public const float MIN_INTERVAL = 5 * MILLI_TO_SECONDS;
        private HashSet<IRumbleSession> _RumbleSessions = new HashSet<IRumbleSession>();
        private float _LastImpulse;
        private Controller _Controller;

        private SteamVR_Action_Vibration hapticAction = SteamVR_Actions.default_Haptic;
        SteamVR_Input_Sources targetSource = SteamVR_Input_Sources.Any;
        float amplitude = 1.0f;
        float frequency = 0.0f;

        protected override void OnStart()
        {
            base.OnStart();

            _Controller = GetComponent<Controller>();
        }

        protected virtual void OnDisable()
        {
            _RumbleSessions.Clear();
        }


        protected override void OnUpdate()
        {
            base.OnUpdate();
            if (_RumbleSessions.Count > 0)
            {
                var session = _RumbleSessions.Max();
                float timeSinceLastImpulse = Time.unscaledTime - _LastImpulse;

                if (_Controller.Tracking.isValid && timeSinceLastImpulse >= session.MilliInterval * MILLI_TO_SECONDS && timeSinceLastImpulse > MIN_INTERVAL)
                {

                    if (session.IsOver)
                    {
                        _RumbleSessions.Remove(session);
                    }
                    else
                    {
                        if (VR.Settings.Rumble)
                        {
                            float durationSeconds = session.MicroDuration / 1000000f;
                            //SteamVR_Controller.Input((int)_Controller.Tracking.index).TriggerHapticPulse(session.MicroDuration);
                            hapticAction.Execute(0f, durationSeconds, frequency, amplitude, targetSource);
                        }
                        _LastImpulse = Time.unscaledTime;
                        session.Consume();
                    }
                }
            }
        }

        public void StartRumble(IRumbleSession session)
        {
            _RumbleSessions.Add(session);
        }

        internal void StopRumble(IRumbleSession session)
        {
            _RumbleSessions.Remove(session);
        }
    }
}
