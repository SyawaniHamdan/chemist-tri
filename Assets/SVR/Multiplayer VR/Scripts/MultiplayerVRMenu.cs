using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Events;
#endif

#if SVR_PHOTON_UNITY_NETWORKING_SDK
using Photon.Pun;
using Photon.Realtime;
#endif

namespace SVR {
    public class MultiplayerVRMenu : MonoBehaviour
    {
#if UNITY_EDITOR
#if SVR_PHOTON_UNITY_NETWORKING_SDK

        // Generate a MultiVR Player spawner in the scene with default settings
        [MenuItem("SVR/Multiplayer VR/VR Player/Spawner", false, 200)]
        public static void MultiplayerVRPlayerSpawner()
        {
            if (GameObject.FindObjectOfType<Camera>())
            {
                DestroyImmediate(GameObject.FindObjectOfType<Camera>().gameObject);
            }

            GameObject go = Instantiate(Resources.Load<GameObject>("SVR Multiplayer Spawner")) as GameObject;
            go.name = "SVR Multiplayer Spawner";
            Selection.activeGameObject = go;
        }

        // Setup the attached script and components to the multiVR player prefab
        [MenuItem("SVR/Multiplayer VR/VR Player/Auto Setup", false, 201)]
        public static void MultiplayerVRPlayerSetup()
        {
            GameObject player = Resources.Load<GameObject>("SVR Multiplayer Player");

            if (!player.GetComponent<PhotonView>())
                player.AddComponent<PhotonView>();

            if (!player.GetComponent<PhotonTransformView>())
                player.AddComponent<PhotonTransformView>();

            PhotonView _photonView = player.GetComponent<PhotonView>();
            NetworkObject _networkObject = player.GetComponent<NetworkObject>();
            PhotonTransformView _photonTransformView = player.GetComponent<PhotonTransformView>();
            MultiplayerVRSerializeMovement _multiplayerVRSerializeMovement = player.GetComponent<MultiplayerVRSerializeMovement>();

            _photonView.ObservedComponents = new List<Component>() { _networkObject, _photonTransformView, _multiplayerVRSerializeMovement };
            _photonView.Synchronization = ViewSynchronization.UnreliableOnChange;

            Debug.Log("[SVR]: Multiplayer Player prefab successfully setup");
        }

        // Create a playable prefab for multiplayer VR settings
        [MenuItem("SVR/Multiplayer VR/Object/Create your own", false, 202)]
        public static void MultiplayerVRObjectInteraction()
        {
            // To exclude the game object in asset folder
            if (Selection.assetGUIDs.Length > 0)
            {
                EditorUtility.DisplayDialog("No object selected", "You must select a game object in the scene hierarchy to be make it as playable object.", "OK");
                return;
            }

            GameObject target = Selection.activeGameObject;

            if (!target.GetComponent<VRInteraction>())
                target.AddComponent<VRInteraction>();

            target.GetComponent<VRInteraction>().controllerInput = target.AddComponent<VRHandControllerInput>();
            target.GetComponent<VRHandControllerInput>().VRButton = GENERIC_VR_BUTTON.STEAMVR_RIGHT_TRIGGER_TOUCH;

            if (!target.GetComponent<PhotonView>())
                target.AddComponent<PhotonView>();

            if (!target.GetComponent<PhotonTransformView>())
                target.AddComponent<PhotonTransformView>();

            if (!target.GetComponent<PhotonRigidbodyView>())
                target.AddComponent<PhotonRigidbodyView>();

            if (!target.GetComponent<NetworkObject>())
                target.AddComponent<NetworkObject>();

            PhotonView _photonView = target.GetComponent<PhotonView>();
            NetworkObject _networkObject = target.GetComponent<NetworkObject>();
            PhotonRigidbodyView _photonRigidbodyView = target.GetComponent<PhotonRigidbodyView>();
            PhotonTransformView _photonTransformView = target.GetComponent<PhotonTransformView>();

            _photonView.ObservedComponents = new List<Component>() { _networkObject, _photonRigidbodyView, _photonTransformView };
            _photonView.OwnershipTransfer = OwnershipOption.Takeover;
            _photonView.Synchronization = ViewSynchronization.UnreliableOnChange;

            target.GetComponent<VRInteraction>().onInteractionStart = new UnityEngine.Events.UnityEvent();
            UnityEventTools.AddVoidPersistentListener(target.GetComponent<VRInteraction>().onInteractionStart, _networkObject.ChangeOwner);

            // Make it into prefab
            string localPath = "Assets/SVR/Multiplayer VR/Resources/Interaction Objects/" + target.name + ".prefab";

            // Make sure the file name is unique, in case an existing Prefab has the same name.
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            // Create the new Prefab.
            PrefabUtility.SaveAsPrefabAssetAndConnect(target, localPath, InteractionMode.UserAction);

            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(localPath);

            Object.DestroyImmediate(target);

            Debug.Log("[SVR]: Interaction Objects successfully created");
        }

        // Create a playable prefab for multiplayer VR settings
        [MenuItem("SVR/Multiplayer VR/Object/Setup Demo Scene", false, 203)]
        public static void MultiplayerVRObjectInteractionDemo()
        {
            GameObject _cube = Resources.Load<GameObject>("Interaction Objects/Cube") as GameObject;
            GameObject _ball = Resources.Load<GameObject>("Interaction Objects/Ball") as GameObject;

            if (!_cube.GetComponent<PhotonView>())
                _cube.AddComponent<PhotonView>();

            if (!_cube.GetComponent<PhotonTransformView>())
                _cube.AddComponent<PhotonTransformView>();


            if (!_cube.GetComponent<PhotonRigidbodyView>())
                _cube.AddComponent<PhotonRigidbodyView>();

            if (!_ball.GetComponent<PhotonView>())
                _ball.AddComponent<PhotonView>();

            if (!_ball.GetComponent<PhotonTransformView>())
                _ball.AddComponent<PhotonTransformView>();

            if (!_ball.GetComponent<PhotonRigidbodyView>())
                _ball.AddComponent<PhotonRigidbodyView>();

            PhotonView _cubePhotonView = _cube.GetComponent<PhotonView>();
            PhotonView _ballPhotonView = _ball.GetComponent<PhotonView>();

            _cubePhotonView.ObservedComponents = new List<Component>() { _cube.GetComponent<PhotonTransformView>(), _cube.GetComponent<NetworkObject>(), _cube.GetComponent<PhotonRigidbodyView>() };
            _cubePhotonView.OwnershipTransfer = OwnershipOption.Takeover;
            _cubePhotonView.Synchronization = ViewSynchronization.UnreliableOnChange;

            _ballPhotonView.ObservedComponents = new List<Component>() { _ball.GetComponent<PhotonTransformView>(), _ball.GetComponent<NetworkObject>(), _ball.GetComponent<PhotonRigidbodyView>() };
            _ballPhotonView.OwnershipTransfer = OwnershipOption.Takeover;
            _ballPhotonView.Synchronization = ViewSynchronization.UnreliableOnChange;

            Debug.Log("[SVR]: Demo scene objects successfully setup");
        }

        // Open the sample scene (lobby scene)
        [MenuItem("SVR/Multiplayer VR/Sample Scene/Lobby", false, 204)]
        public static void MultiplayerVRLobbyScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/SVR/Multiplayer VR/Scenes/MVR Lobby Scene.unity");
        }

        // Open the sample scene (game scene)
        [MenuItem("SVR/Multiplayer VR/Sample Scene/Game", false, 205)]
        public static void MultiplayerVRGameScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/SVR/Multiplayer VR/Scenes/MVR Game Scene.unity");
        }
#endif
#endif
    }

}

