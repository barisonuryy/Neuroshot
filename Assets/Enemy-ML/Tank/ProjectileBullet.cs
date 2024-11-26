
using System;
using UnityEngine;


public class ProjectileBullet : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;
    private Vector3 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
      
        
        // Mermiyi yönlendirecek kuvveti uygula
        //rb.velocity = transform.forward * speed;
    }


    public void Initialize(Vector3 target)
    {
        direction = target;
        Debug.Log("Değer "+direction);
    }

    private void FixedUpdate()
    {
        rb.velocity = direction* speed*Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Oyuncuya hasar ver veya gerekli işlemi yap
            Destroy(gameObject); // Mermiyi yok et
        }
    }
}
