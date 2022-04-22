using UnityEngine;

public class BulletScript : MonoBehaviour
{
    
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    
}
