// This script it written to serialize the hand movement 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

#if SVR_PHOTON_UNITY_NETWORKING_SDK
using Photon.Pun;
#endif

namespace SVR
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class MultiplayerVRHandController : VRHandController
    {

#if SVR_PHOTON_UNITY_NETWORKING_SDK
        private PhotonView photonView;
#endif

        //public XRNode node;

        //public InputMode inputMode;

        //[Tooltip("Enable if using hand controller for automatic adjustment")]
        //public bool isHand;

        //private Transform otherParent;

        private Transform interactInitialPositionM;

        private VRInteraction interactingObjectM;

        //private Vector3 velocity;
        //private Vector3 prevPosition;
        //private Vector3 deltaPosition;

        //private int frameCount;

        private void Awake()
        {
            //frameCount = 0;
            //velocity = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            photonView = transform.parent.gameObject.GetComponent<PhotonView>();

            if (!photonView || !photonView.IsMine)
                return;

            //prevPosition = transform.position;
            interactingObjectM = null;

            if (isHand)
            {
                string[] joystickNames = Input.GetJoystickNames();
                //Debug.Log(joystickNames.Length);

                for (int i = 0; i < joystickNames.Length; i++)
                {
                    //Debug.Log(joystickNames[i].ToString());
                    if (joystickNames[i].ToLower().Contains("left") && name.ToLower().Contains("left"))
                    {
                        node = XRNode.LeftHand;
                        //Debug.Log(node.ToString());
                    }
                    if (joystickNames[i].ToLower().Contains("right") && name.ToLower().Contains("right"))
                    {
                        node = XRNode.RightHand;
                        //Debug.Log(node.ToString());
                    }
                }
            }
#endif
        }

        // Update is called once per frame
        [System.Obsolete]
        void Update()
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            if (!photonView || !photonView.IsMine)
                return;
            
            transform.localPosition = InputTracking.GetLocalPosition(node);
            transform.localRotation = InputTracking.GetLocalRotation(node);
#endif
        }

        public void OnTriggerEnter(Collider other)
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            if (!photonView || !photonView.IsMine)
                return;

            if (other.GetComponent<VRInteraction>() && other.GetComponent<VRHandControllerInput>())
            {
                // The object we are interacting must have VR Interaction and Controller Input scripts
                VRHandControllerInput otherInput = other.GetComponent<VRHandControllerInput>();
                VRInteraction otherInteraction = other.GetComponent<VRInteraction>();

                otherInteraction.showHighlight(true);
            }
            else if (other.GetComponent<VRUIInteraction>() && other.GetComponent<VRHandControllerInput>())
            {
                // The object we are interacting must have VR Interaction and Controller Input scripts
                VRHandControllerInput otherInput = other.GetComponent<VRHandControllerInput>();
                VRUIInteraction otherInteraction = other.GetComponent<VRUIInteraction>();

                otherInteraction.Enter();
            }
#endif
        }

        public void OnTriggerStay(Collider other)
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            if (!photonView || !photonView.IsMine)
                return;

            if (other.GetComponent<VRInteraction>() && other.GetComponent<VRHandControllerInput>())
            {
                // The object we are interacting must have VR Interaction and Controller Input scripts
                VRHandControllerInput otherInput = other.GetComponent<VRHandControllerInput>();
                VRInteraction otherInteraction = other.GetComponent<VRInteraction>();

                if (interactingObjectM != null && interactingObjectM != otherInteraction) return;

                if (otherInput.GetInputKeyStart())
                {

                    interactingObjectM = otherInteraction;
                    // Make the call back to this on start
                    otherInteraction.OnBeginInteraction();

                    other.GetComponent<Rigidbody>().useGravity = false;

                    interactInitialPositionM = transform;

                    if (otherInteraction.hideHandOnInteract)
                    {
                        try
                        {
                            transform.Find("Model").GetComponent<MeshRenderer>().enabled = false;
                        }
                        catch (System.NullReferenceException)
                        {
                            Debug.Log("No Mesh Renderer on hand model found");
                        }
                    }
                }

                if (otherInput.GetInputKey())
                {
                    // Make the call back to this on update
                    otherInteraction.OnInteract(transform, interactInitialPositionM);
                }

                if (otherInput.GetInputKeyEnd())
                {
                    interactingObjectM = null;
                    // Make the call back to this on end
                    otherInteraction.OnFinishInteraction();

                    if (!otherInteraction.backToOriginalPosition && otherInteraction.canBeThrow)
                        other.GetComponent<Rigidbody>().useGravity = true;

                    if (otherInteraction.hideHandOnInteract)
                    {
                        try
                        {
                            transform.Find("Model").GetComponent<MeshRenderer>().enabled = true;
                        }
                        catch (System.NullReferenceException)
                        {
                            Debug.Log("No Mesh Renderer on hand model found");
                        }

                    }
                }
            }
            else if (other.GetComponent<VRUIInteraction>() && other.GetComponent<VRHandControllerInput>())
            {
                VRHandControllerInput otherInput = other.GetComponent<VRHandControllerInput>();
                VRUIInteraction otherInteraction = other.GetComponent<VRUIInteraction>();

                if (otherInput.GetInputKeyStart())
                {
                    otherInteraction.Trigger();
                }
            }
#endif
        }

        public void OnTriggerExit(Collider other)
        {
#if SVR_PHOTON_UNITY_NETWORKING_SDK
            if (!photonView || !photonView.IsMine)
                return;

            if (other.GetComponent<VRInteraction>() && other.GetComponent<VRHandControllerInput>())
            {
                // The object we are interacting must have VR Interaction and Controller Input scripts
                VRHandControllerInput otherInput = other.GetComponent<VRHandControllerInput>();
                VRInteraction otherInteraction = other.GetComponent<VRInteraction>();

                otherInteraction.showHighlight(false);
            }
            else if (other.GetComponent<VRUIInteraction>() && other.GetComponent<VRHandControllerInput>())
            {
                // The object we are interacting must have VR Interaction and Controller Input scripts
                VRHandControllerInput otherInput = other.GetComponent<VRHandControllerInput>();
                VRUIInteraction otherInteraction = other.GetComponent<VRUIInteraction>();

                otherInteraction.Exit();
            }
#endif
        }

    }

}
