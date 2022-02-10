// This script is written to attach a camera to the player after entering the scene
// Each player will have one camera at their own game without interfering the other player
// If 2 cameras are in the same scene, the replacement will occurred (undesired)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SVR_PHOTON_UNITY_NETWORKING_SDK
using Photon.Pun;
#endif

namespace SVR
{
    public class MultiplayerVRCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            if (transform.parent.GetComponent<PhotonView>().IsMine)
            {
                if (!GetComponent<Camera>())
                {
                    Camera _camera = gameObject.AddComponent<Camera>();
                    _camera.nearClipPlane = 0.01f;
                    gameObject.AddComponent<FlareLayer>();
                    gameObject.AddComponent<AudioListener>();
                }
            }
#endif
        }
    }
}