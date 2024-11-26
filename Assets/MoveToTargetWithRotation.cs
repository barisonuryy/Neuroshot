using UnityEngine;

public class MoveToTargetWithRotation : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float rotationSpeed = 720f; // Derece/saniye

    void Update()
    {
        if (target != null)
        {
            // Hedefe doğru yönel
            Vector3 direction = (target.position - transform.position).normalized;

            // Rotasyonu hesapla
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Pozisyonu hedefe doğru hareket ettir
            transform.position += transform.forward * speed * Time.deltaTime;

            // Eğer hedefe çok yakınsa, dur
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.1f)
            {
                Debug.Log("Hedefe ulaşıldı!");
            }
        }
    }
}