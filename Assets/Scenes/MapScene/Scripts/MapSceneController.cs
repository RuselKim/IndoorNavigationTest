using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using UnityEngine.UI;

    public class MapSceneController : MonoBehaviour
    {
        public Text camPoseText;
        public Camera m_firstPersonCamera;
        public GameObject cameraTarget;
        private Vector3 m_prevARPosePosition;
        private bool trackingStarted = false;
    
        public void Start() {
            m_prevARPosePosition = Vector3.zero;
        }

        public void Update(){
           _QuitOnConnectionErrors();
           
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Vector3 currentARPosition = Frame.Pose.position;
            if (!trackingStarted) {
                trackingStarted = true;
                m_prevARPosePosition = Frame.Pose.position;
            }
            //Remember the previous position so we can apply deltas
            Vector3 deltaPosition = currentARPosition - m_prevARPosePosition;
            m_prevARPosePosition = currentARPosition;
            if (cameraTarget != null) {
                // The initial forward vector of the sphere must be aligned with the initial camera direction in the XZ plane.
                // We apply translation only in the XZ plane.
                cameraTarget.transform.Translate (deltaPosition.x, 0.0f, deltaPosition.z);  
                // Set the pose rotation to be used in the CameraFollow script
                m_firstPersonCamera.GetComponent<FollowTarget> ().targetRot = Frame.Pose.rotation;
            }
        }

        private void _QuitOnConnectionErrors() {
            // Do not update if ARCore is not tracking.
            if (Session.Status == SessionStatus.ErrorApkNotAvailable) {
                camPoseText.text = "This device does not support ARCore.";
                Application.Quit();
            }
            else if (Session.Status == SessionStatus.ErrorPermissionNotGranted) {
                camPoseText.text = "Camera permission is needed to run this application.";
                Application.Quit();
            }
            else if (Session.Status == SessionStatus. ErrorSessionConfigurationNotSupported) {
                camPoseText.text = "ARCore encountered a problem connecting.  Please start the app again.";
                Application.Quit();
            }
     }
}
    

