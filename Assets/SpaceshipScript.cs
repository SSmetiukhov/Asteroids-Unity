using System.Collections;
using UnityEngine;

public class SpaceshipScript : MonoBehaviour
{
    public float rotationSpeed = 200f;
    private float bulletSpeed = 1500f;
    private float deadZone = 1.06f ;
    public GameObject bullet;
    public float fireRate = 0.25f;
    public float nextFire;
   


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        StartCoroutine(Carousel(new Vector3(-37, 0, -18), new Vector3(37, 0, 18), rb));
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Rotate(0, 0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * 5 * Input.GetAxis("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            Shoot();
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

            transform.position = pos;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Shoot()
    {
        nextFire = Time.time + fireRate;
        var transform1 = transform;
        var newBullet = Instantiate(bullet, transform1.position, transform1.rotation);
        newBullet.GetComponent<Rigidbody>().AddForce(transform1.up * bulletSpeed);
    }
}
