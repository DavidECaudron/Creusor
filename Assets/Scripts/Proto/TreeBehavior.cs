using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    public class TreeBehavior : MonoBehaviour
    {
        List<Rigidbody> rbFragments = new List<Rigidbody>();
        List<Rigidbody> rbLeaves = new List<Rigidbody>();
        MeshRenderer treeRenderer;
        //MeshCollider treeCollider;
        GameObject shatteredGroup;
        GameObject leavesGroup;
        public float thrust = 10f;

        //Change to DigMask Pos (Send as parameter when trigger call "DestroyTree()")
        public Transform digMask;

        bool hideFragments = false;

        float timer = 0.5f;
        float elapsedTime = 0;





        // Start is called before the first frame update

       void Awake()
        {
            treeRenderer = GetComponent<MeshRenderer>();
            //treeCollider = GetComponent<MeshCollider>();
            shatteredGroup = transform.GetChild(0).gameObject;
            leavesGroup = transform.GetChild(1).gameObject;

            for(int i = 0; i < shatteredGroup.transform.childCount; i++)
            {
                rbFragments.Add(shatteredGroup.transform.GetChild(i).GetComponent<Rigidbody>());
            }

            for(int i = 0; i < leavesGroup.transform.childCount; i++)
            {
                rbLeaves.Add(leavesGroup.transform.GetChild(i).GetComponent<Rigidbody>());
            }

            shatteredGroup.SetActive(false);
        }
        void Start()
        {
            DestroyTree(digMask.transform.position);
        }

        // Update is called once per frame
        void Update()
        {
            if(hideFragments)
            {
                if (elapsedTime < timer)
                {
                    Vector3 lerpFragments = Vector3.Lerp (transform.localScale, Vector3.zero, elapsedTime / timer);
                    float yLerpFragments = Mathf.Lerp(transform.localScale.y, 1.5f, elapsedTime / timer);
                    Vector3 lerpLeaves = Vector3.Lerp (new Vector3(0.01f, 0.01f, 0.01f), Vector3.zero, elapsedTime / timer);

                    for(int i = 0; i < rbFragments.Count; i++)
                    {
                        rbFragments[i].transform.localScale = new Vector3(lerpFragments.x, yLerpFragments , lerpFragments.z);
                    }
                    for(int i = 0; i < rbLeaves.Count; i++)
                    {
                        rbLeaves[i].transform.localScale = lerpLeaves;
                    }

                    elapsedTime += Time.deltaTime;
                }
            }
        }

        public void DestroyTree(Vector3 digMaskPos)
        {
            treeRenderer.enabled  = false;
            //treeCollider.enabled = false;
            shatteredGroup.SetActive(true);

            Vector3 forceDirection = (transform.position - digMaskPos).normalized;

            for(int i = 0; i < rbFragments.Count; i++)
            {
                rbFragments[i].isKinematic = false;
                float fragmentPosY = rbFragments[i].transform.position.y;
                rbFragments[i].AddRelativeForce(new Vector3(forceDirection.x * thrust,  fragmentPosY, forceDirection.z * thrust));
            }

            for(int i = 0; i < rbLeaves.Count; i++)
            {
                rbLeaves[i].isKinematic = false;
            }



            StartCoroutine(DestroyTreeObject());
        }
        IEnumerator DestroyTreeObject()
        {
            yield return new WaitForSeconds(0.15f);
            hideFragments = true;
            yield return new WaitForSeconds(timer);

            Destroy(gameObject);
            yield return null;
        }
    }
}
