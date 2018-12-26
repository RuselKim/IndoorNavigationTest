using UnityEngine;
// Attach this script to the camera that you want to follow the target
public class FollowTargetArMode : MonoBehaviour {
    public Transform targetToFollow;
    private Quaternion startRot;
    private Gyroscope gyro;
    private bool gyroEnabled;


    void Start () {
        gyroEnabled = EnableGyro();
     }

    // Use lateUpdate to assure that the camera is updated after the target has been updated.
    void  LateUpdate () {
       if (!targetToFollow)
            return;

        // Set camera position the same as the target position
        transform.position = targetToFollow.position;

        if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * startRot;
        }
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            startRot = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }
}
