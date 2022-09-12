using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    public class CameraFollow : MonoBehaviour
    {
        #region Inspector

        public Transform target;

        public float SmoothFactor = 1f;
        public static bool followTarget = true;

        #endregion


        #region Hidden

        private Camera mainCamera;

        #endregion


        #region Updates

        void Awake()
        {
            mainCamera = Camera.main;

            DontDestroyOnLoad(this.gameObject);
        }

        void FixedUpdate()
        {
            if (followTarget)
            {
                Vector3 smoothPos = Vector3.Slerp(transform.position, target.position, SmoothFactor * Time.deltaTime);
                transform.position = smoothPos;
            }
        }

        #endregion


        #region Main

        public void CameraZoom(bool isZoomed)
        {
            StartCoroutine(ChangeCameraZoom(isZoomed));
        }

        #endregion


        #region Utils

        public IEnumerator CameraShake(float duration, float magnitude)
        {
            followTarget = false;

            Vector3 originalPos = transform.position;

            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                float x = Random.Range(originalPos.x - 1f, originalPos.x + 1f) * magnitude;
                float y = Random.Range(originalPos.y - 1f, originalPos.y + 1f) * magnitude;

                transform.position = new Vector3(x, y, originalPos.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            transform.position = originalPos;
            followTarget = true;
        }

        public IEnumerator ChangeCameraZoom(bool isZoomed)
        {
            followTarget = false;

            float duration = 0.3f;
            float elapsed = 0.0f;

            float startCameraDist = mainCamera.orthographicSize;
            float endCameraDist = startCameraDist;

            if (!isZoomed)
            {
                endCameraDist /= 2f;
            }
            else
            {
                endCameraDist *= 2f;
            }

            while (elapsed < duration)
            {
                mainCamera.orthographicSize = Mathf.Lerp(startCameraDist, endCameraDist, elapsed / duration);
                elapsed += Time.deltaTime;

                yield return null;
            }

            mainCamera.orthographicSize = endCameraDist;
            followTarget = true;
        }

        #endregion
    }
}
