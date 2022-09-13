using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace refacto_deux
{
    public class MiniMap : MonoBehaviour
    {
        public Transform player;
        private Image playerMarker;
        //private Transform[] treasureAreaMarkers;
        //private Vector3[] treasureAreaWorldPosition;
        // Start is called before the first frame update
        void Awake()
        {
            playerMarker = GameObject.Find("PlayerIcon").GetComponent<Image>();
            //treasureAreaMarkers = new Transform[3];
            //treasureAreaMarkers[0] = GameObject.Find("MarkerAnchor_01").transform;
            //treasureAreaMarkers[0] = GameObject.Find("MarkerAnchor_02").transform;
            //treasureAreaMarkers[0] = GameObject.Find("MarkerAnchor_03").transform;

            //treasureAreaMarkers = new Transform[3];
            //treasureAreaMarkers[0] = GameObject.Find("MarkerAnchor_01").transform;
            //treasureAreaMarkers[0] = GameObject.Find("MarkerAnchor_02").transform;
            //treasureAreaMarkers[0] = GameObject.Find("MarkerAnchor_03").transform;
        }

        // Update is called once per frame
        void Update()
        {
            if (player != null)
            {
                Vector3 playerMarkerRotation = playerMarker.transform.eulerAngles;
                playerMarkerRotation.z = player.eulerAngles.y;
                playerMarker.transform.eulerAngles = playerMarkerRotation;
            }
        }

        private void LateUpdate()
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
    }
}
