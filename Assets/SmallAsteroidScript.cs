using System.Collections;
using UnityEngine;

public class SmallAsteroidScript : MonoBehaviour
{
    public Rigidbody rigidBody;
    private float deadZone = 1.06f;
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        // randomise size+mass
        Transform transform1;
        (transform1 = transform).localScale = new Vector3(Random.Range(0.08f,0.12f), 0, Random.Range(0.08f,0.12f));
        var localScale = transform1.localScale;
        rigidBody.mass = localScale.x * localScale.y * localScale.z;
        // randomise velocity
        rigidBody.velocity = new Vector3 (Random.Range (-10f, 10f), 0f, Random.Range (-10f, 10f));
        rigidBody.angularVelocity = new Vector3 (Random.Range (-4f, 4f), 0, Random.Range (-4f, 4f));
        
        StartCoroutine(Carousel(new Vector3(-37, 0, -18), new Vector3(37, 0, 18), rigidBody));
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            GameManagerScript.AddScore(100);
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
    IEnumerator Carousel(Vector3 bottomLeftCorner, Vector3 topRightCorner, Rigidbody rb) {
        
        var teleportBottomLeftCorner = bottomLeftCorner * deadZone;
        var teleportTopRightCorner = topRightCorner * deadZone;
         
        while (true) {
            Vector3 pos = transform.position;
            Vector3 vel = rb.velocity;

            /*
             Yeah turns out that you kinda need that velocity check.
             */
             
            if (pos.x < teleportBottomLeftCorner.x && vel.x < 0) {
                pos.x = teleportTopRightCorner.x;
                
            } 
            else if (pos.x > teleportTopRightCorner.x && vel.x > 0) {
                pos.x = teleportBottomLeftCorner.x;
            }

            if (pos.z < teleportBottomLeftCorner.z && vel.z < 0) {
                pos.z = teleportTopRightCorner.z;
            }
            else if (pos.z > teleportTopRightCorner.z && vel.z > 0) {
                pos.z = teleportBottomLeftCorner.z;
            }

            pos.y = 0;

            transform.position = pos;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
