using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public Transform target; // Hedef nesnesi
    public float speed = 5f; // Hareket hızı

    private Rigidbody rb;

    void Start()
    {
        // Rigidbody bileşenini al
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody bileşeni bulunamadı!");
        }
    }

    void FixedUpdate()
    {
        if (target != null && rb != null)
        {
            // Hedefe doğru yönel
            Vector3 direction = (target.position - transform.position).normalized;

            // Rigidbody'ye kuvvet uygula
            rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

            // Eğer hedefe çok yakınsa, dur
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.1f)
            {
                Debug.Log("Hedefe ulaşıldı!");
                rb.velocity = Vector3.zero; // Hızı sıfırla
            }
        }
    }
}