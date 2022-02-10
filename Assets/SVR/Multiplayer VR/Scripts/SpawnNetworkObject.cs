using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVR {
    public class SpawnNetworkObject : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<NetworkObjectHandler>().SpawnNetworkObject();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}