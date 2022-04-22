using System.Collections;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    public float lowerForceRange = 10f;
    public float upperForceRange = 20f;

    public GameObject smallAsteroidPrefab;
    
    private float deadZone = 1.06f;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * i have 4 spawn lines (see schema below)
         * range describes a point on the spawn line
         * direction describes with which spawn box we will work with (top(1) or bottom(3), right(2) or left(4))
         * side describes the direction we should lounge our asteroid so it won't go off screen (see arrows on the schema)
         * and I don't understand why the weight of an object was effected so I meaninglessly spent 4 hours debugging
         * switch itself chooses on what group of spawn lines we will operate (1 for top(1) or bottom(3), 2 for right(2) or left(4))
         * 
         * I introduced all these variables so that I can fit my code in 2 cases, because my first solution with 8 switches looked gross,
         * while 1 case solution was too over engineered
         */
        float range;
        int direction;
        int side = Random.Range(0, 2) * 2 - 1;
        

        Rigidbody rb = GetComponent<Rigidbody>();
        switch (Random.Range(1, 3))
        {
            // SPAWN LINE top(1) or bottom(3)
            case 1:
                range = Random.Range(-37f, 37f);
                transform.position = new Vector3(range, 0f, 19f * side);
                direction = range < 0 ? 1 : -1;
                rb.AddForce(new Vector3(
                    Random.Range(lowerForceRange, upperForceRange) * direction,
                    0,
                    Random.Range(lowerForceRange, upperForceRange) * -50 * side));
                break;
            // SPAWN LINE right(2) or left(4)
            case 2:
                range = Random.Range(-18f, 18f);
                transform.position = new Vector3(38f * side, 0, range);
                direction = range < 0 ? 1 : -1;
                rb.AddForce(new Vector3(
                    Random.Range(lowerForceRange, upperForceRange) * -50 * side,
                    0,
                    Random.Range(lowerForceRange, upperForceRange) * direction));
                break;
        }

        StartCoroutine(Carousel(new Vector3(-37, 0, -18), new Vector3(37, 0, 18), rb));
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            var position = transform.position;
            GameObject asteroid1 = Instantiate(smallAsteroidPrefab);
            asteroid1.transform.position = position + Vector3.right * 2;
            GameObject asteroid2 = Instantiate(smallAsteroidPrefab);
            asteroid2.transform.position = position + Vector3.left * 2;
            
            GameManagerScript.AddScore(100);
        }

        if (other.gameObject.CompareTag("ship"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            GameManagerScript.Instance.CreatePlayerSpaceship();
        }
    }
    
}