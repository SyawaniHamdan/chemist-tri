// This script is written to serialize the movement of all the hierarchy objects (child object) by using Photon Serialize View

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if SVR_PHOTON_UNITY_NETWORKING_SDK
using Photon.Pun;
using Photon.Realtime;
#endif

namespace SVR
{
#if SVR_PHOTON_UNITY_NETWORKING_SDK
    [RequireComponent(typeof(PhotonView))]
#endif
    public class MultiplayerVRSerializeMovement :
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            MonoBehaviourPunCallbacks, IPunObservable
#else
            MonoBehaviour
#endif
    {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
        PhotonView _photonView;
#endif

        public bool syncPosition = true;
        public bool syncRotation = true;
        public bool syncScale = false;

        [Tooltip("The child components that need to serialize their transformation")]
        public Transform[] parts;

        private List<float> distance = new List<float>();
        private List<float> angle = new List<float>();

        private List<Vector3> direction = new List<Vector3>();
        private List<Vector3> networkPosition = new List<Vector3>();
        private List<Vector3> storedPosition = new List<Vector3>();

        private List<Quaternion> networkRotation = new List<Quaternion>();

        List<bool> firstTake = new List<bool>();

        // Start is called before the first frame update
        void Start()
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            _photonView = GetComponent<PhotonView>();

#endif

        }

        void OnEnable()
        {
            for(int i=0; i < parts.Length; i++)
            {
                firstTake.Add(true);
            }


            foreach (Transform _part in parts)
            {
                storedPosition.Add(_part.position);
                networkPosition.Add(Vector3.zero);
                networkRotation.Add(Quaternion.identity);

                direction.Add(Vector3.zero);

                distance.Add(0f);
                angle.Add(0f);

            }
        }


        public void Update()
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            if (!_photonView.IsMine)
            {
                for(int i=0; i<parts.Length; i++)
                {
                    parts[i].position = Vector3.MoveTowards(parts[i].position, networkPosition[i], distance[i] * (1.0f / PhotonNetwork.SerializationRate));
                    parts[i].rotation = Quaternion.RotateTowards(parts[i].rotation, networkRotation[i], angle[i] * (1.0f / PhotonNetwork.SerializationRate));
                }
            }
#endif
        }

#if SVR_PHOTON_UNITY_NETWORKING_SDK
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // stream.SendNext(data(type))

                for(int i=0; i < parts.Length; i++)
                {
                    if (syncPosition)
                    {
                        direction[i] = parts[i].position - storedPosition[i];
                        storedPosition[i] = parts[i].position;

                        stream.SendNext(parts[i].position);
                        stream.SendNext(direction[i]);
                    }

                    if (syncRotation)
                    {
                        stream.SendNext(parts[i].rotation);
                    }

                    if (syncScale)
                    {
                        stream.SendNext(parts[i].localScale);
                    }
                }

            }
            else
            {
                // (cast data type)stream.ReceiveNext(), need to be in correct order

                for(int i=0; i<parts.Length; i++)
                {
                    if (syncPosition)
                    {
                        networkPosition[i] = (Vector3)stream.ReceiveNext();
                        direction[i] = (Vector3)stream.ReceiveNext();

                        if (firstTake[i])
                        {
                            parts[i].position = networkPosition[i];
                            distance[i] = 0f;
                        }
                        else
                        {
                            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                            networkPosition[i] += direction[i] * lag;
                            distance[i] = Vector3.Distance(parts[i].position, networkPosition[i]);
                        }
                    }

                    if (syncRotation)
                    {
                        networkRotation[i] = (Quaternion)stream.ReceiveNext();

                        if (firstTake[i])
                        {
                            angle[i] = 0f;
                            parts[i].rotation = networkRotation[i];
                        }
                        else
                        {
                            angle[i] = Quaternion.Angle(parts[i].rotation, networkRotation[i]);
                        }
                    }

                    if (syncScale)
                    {
                        parts[i].localScale = (Vector3)stream.ReceiveNext();
                    }


                    if (firstTake[i])
                    {
                        firstTake[i] = false;
                    }
                }
            }
        }
#endif
    }
}
