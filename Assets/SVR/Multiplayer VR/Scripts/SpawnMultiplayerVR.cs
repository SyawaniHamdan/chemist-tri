using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SVR_PHOTON_UNITY_NETWORKING_SDK
using Photon.Pun;
#endif

namespace SVR
{
    public class SpawnMultiplayerVR : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            GameObject multiplayerPlayer = GetComponent<NetworkObjectHandler>().SpawnNetworkObject();
            PhotonView photonView;
            //PhotonTransformView photonTransformView;

            if (!multiplayerPlayer.GetComponent<PhotonView>())
            {
                photonView = multiplayerPlayer.AddComponent<PhotonView>();
            }
            else
            {
                photonView = multiplayerPlayer.GetComponent<PhotonView>();
            }

            //if (!multiplayerPlayer.GetComponent<PhotonTransformView>())
            //{
            //    photonTransformView = multiplayerPlayer.AddComponent<PhotonTransformView>();
            //}
            //else
            //{
            //    photonTransformView = multiplayerPlayer.GetComponent<PhotonTransformView>();
            //}

            //photonTransformView.m_SynchronizePosition = true;
            //photonTransformView.m_SynchronizeRotation = true;

            //List<Component> components = new List<Component>();
            //components.Add(photonTransformView);
            //photonView.ObservedComponents = components;

            VRTeleportPointer teleportPointer = GameObject.FindObjectOfType<VRTeleportPointer>();
            if (teleportPointer)
            {
                if(photonView.IsMine)
                    teleportPointer.controller = multiplayerPlayer.transform.Find("Right Controller").GetComponent<MultiplayerVRHandController>();
            }
#endif

        }
    }
}