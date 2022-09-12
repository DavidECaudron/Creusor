using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    [ExecuteInEditMode]
    public class AbiliyRangeDash : MonoBehaviour
    {
        [Range(1,20)] public float dashDistance = 2f;
        [Range(1,3)] public float minDistanceToDash = 1.5f;
        Vector3 newTargetPos;
        Vector3 enterInGroundPos;
        public Transform player;
        MeshRenderer playerRend;

        public GameObject tunelExitPrefab; 

        void Start()
        {
            playerRend = player.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
            StartCoroutine(Dash());
            //newTargetPos = transform.GetChild(1).localPosition;
        }

        // Update is called once per frame
        void Update()
        {

        }
        void OnDrawGizmosSelected()
        {

            transform.GetChild(1).localPosition = new Vector3(0,0,dashDistance);
            newTargetPos = transform.GetChild(1).position;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(newTargetPos, 0.5f);
            Vector3 initialLinePos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
            Gizmos.DrawLine(initialLinePos, new Vector3(newTargetPos.x, newTargetPos.y, newTargetPos.z));            
        }

        IEnumerator DisplayExitHole(Vector3 tunelExitPos)
        {
            float elapsedTime = 0;
            float duration = 0.5f;

            GameObject exitInstance = Instantiate(tunelExitPrefab, tunelExitPos, Quaternion.identity);
            exitInstance.transform.localScale = Vector3.one;

            while(elapsedTime < duration)
            {
                exitInstance.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }            

            yield return new WaitForSeconds(0.75f);

            elapsedTime = 0;
            duration = 0.25f;

            while(elapsedTime < duration)
            {
                exitInstance.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            } 

            Destroy(exitInstance);



            yield return null;
        }
        IEnumerator Dash()
        {
            yield return new WaitForSeconds(3f);

            gameObject.transform.parent = null;

            float elapsedTime = 0;
            float duration = 0.1f;


            Vector3 startPos = player.transform.position;
            Vector3 tunelExit = transform.GetChild(1).position;

            Vector3 targetPos = startPos + (tunelExit - startPos) * 0.15f;

            Vector3 playerScale = player.transform.localScale;
            Vector3 targetScale = playerScale * 0.7f;            
            //targetPos.y = -3f;

            //A+(B-A)*0.2
            

            while(elapsedTime < duration)
            {
                player.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / duration);
                player.transform.localScale = Vector3.Lerp(playerScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            playerRend.enabled = false;

            Vector3 inGroundPos = new Vector3(tunelExit.x, -4f, tunelExit.z);
            player.transform.position = inGroundPos;


            yield return new WaitForSeconds(0.2f); //Dash fx duration

            //TunelOpening fx


            yield return new WaitForSeconds(0.2f); //Dash fx duration

            StartCoroutine(DisplayExitHole(tunelExit));
 
            yield return new WaitForSeconds(0.15f); //Dash fx duration           
            duration = 0.2f;
            elapsedTime = 0;

            playerRend.enabled = true;

            while(elapsedTime < duration)
            {
                player.transform.position = Vector3.Lerp(inGroundPos, tunelExit, elapsedTime / duration);
                player.transform.localScale = Vector3.Lerp(targetScale, playerScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            player.transform.position = tunelExit;
            player.localScale = playerScale;  
            
            gameObject.transform.parent = player;  
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.Euler(0,0,0);  

            yield return null;
        }

    }
}
