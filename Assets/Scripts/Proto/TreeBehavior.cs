using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace proto
{
    public class TreeBehavior : MonoBehaviour
    {
        List<Rigidbody> rbFragments = new List<Rigidbody>();
        List<MeshRenderer> rendFragments = new List<MeshRenderer>();
        List<Rigidbody> rbLeaves = new List<Rigidbody>();
        List<MeshRenderer> rendLeaves = new List<MeshRenderer>();

        Color leavesColor;
        MeshRenderer treeRenderer;
        //MeshCollider treeCollider;
        GameObject shatteredGroup;
        GameObject leavesGroup;
        public float thrust = 10f;

        public bool containCoconuts = false;

        bool hideFragments = false;

        float timer = 0.5f;
        float elapsedTime = 0;

        List<Vector3> coconutSpawnPositions = new List<Vector3>();

        public GameObject coconutPrefab;

        [SerializeField] List<Rigidbody> rbCoconuts = new List<Rigidbody>();
        [SerializeField] List<Animator> animatorCoconuts = new List<Animator>();



        // Start is called before the first frame update

       void Awake()
        {
            treeRenderer = GetComponent<MeshRenderer>();
            //treeCollider = GetComponent<MeshCollider>();
            shatteredGroup = transform.GetChild(0).gameObject;
            leavesGroup = transform.GetChild(1).gameObject;
            leavesColor = transform.GetComponent<VegetationRandomizer>().randomColor;

            for(int i = 0; i < shatteredGroup.transform.childCount; i++)
            {
                rbFragments.Add(shatteredGroup.transform.GetChild(i).GetComponent<Rigidbody>());
                rendFragments.Add(shatteredGroup.transform.GetChild(i).GetComponent<MeshRenderer>());
            }

            for(int i = 0; i < leavesGroup.transform.childCount; i++)
            {
                rbLeaves.Add(leavesGroup.transform.GetChild(i).GetComponent<Rigidbody>());
                rendLeaves.Add(leavesGroup.transform.GetChild(i).GetComponent<MeshRenderer>());
                rendLeaves[i].material.SetColor("_RandomColor", leavesColor);
            }

            shatteredGroup.SetActive(false);
            Transform coconutSpawnParent = transform.GetChild(2);

            
            for(int i = 0; i < coconutSpawnParent.childCount; i++)
            {
                coconutSpawnPositions.Add(coconutSpawnParent.GetChild(i).position);
            }

            if(containCoconuts)
            {
                float spawnCoconutRandom = Random.Range(0,1f);
                GameObject coconutInstanceA = Instantiate(coconutPrefab, coconutSpawnPositions[0], Quaternion.identity);
                rbCoconuts.Add(coconutInstanceA.GetComponent<Rigidbody>());
                animatorCoconuts.Add(coconutInstanceA.GetComponent<Animator>());

                if(spawnCoconutRandom > (1f - 0.3f)) //30% chance to spawn a second coconut;
                {
                    GameObject coconutInstanceB = Instantiate(coconutPrefab, coconutSpawnPositions[1], Quaternion.identity);
                    rbCoconuts.Add(coconutInstanceB.GetComponent<Rigidbody>());
                    animatorCoconuts.Add(coconutInstanceB.GetComponent<Animator>());
                }
                if(spawnCoconutRandom > (1f - 0.1f)) //10% chance to spawn a second coconut;
                {
                    GameObject coconutInstanceC = Instantiate(coconutPrefab, coconutSpawnPositions[2], Quaternion.identity);
                    rbCoconuts.Add(coconutInstanceC.GetComponent<Rigidbody>());
                    animatorCoconuts.Add(coconutInstanceC.GetComponent<Animator>());
                }
            }
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

            for(int i = 0; i < rbCoconuts.Count; i++)
            {
                rbCoconuts[i].isKinematic = false;
                animatorCoconuts[i].SetTrigger("Collectable");
            }
            for(int i = 0; i < rbFragments.Count; i++)
            {
                rendFragments[i].enabled = true;
                rbFragments[i].isKinematic = false;
                float fragmentPosY = rbFragments[i].transform.position.y;
                rbFragments[i].AddRelativeForce(new Vector3(forceDirection.x * thrust,  fragmentPosY, forceDirection.z * thrust));
            }

            for(int i = 0; i < rbLeaves.Count; i++)
            {
                rendLeaves[i].enabled = true;
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
