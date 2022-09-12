using UnityEngine;

namespace refacto_deux
{
    [ExecuteAlways]
    public class Projectile : MonoBehaviour
    {
        [Range(1,20)] public float throwDistance = 2f;
        [Range(1,180)] public float throwAngle = 45f;
        Vector3 newThrowHitPos;

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        }
        void OnDrawGizmosSelected()
        {
            newThrowHitPos = new Vector3(transform.position.x, 0.01f, transform.position.z + throwDistance);
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(newThrowHitPos, 0.5f);
            Vector3 initialLinePos = new Vector3 (transform.position.x, 0.01f, transform.GetChild(0).transform.position.z);
            Gizmos.DrawLine(initialLinePos, newThrowHitPos);            
        }
    }
}
