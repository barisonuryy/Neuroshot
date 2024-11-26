using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ArcherDefence : Agent
{
    [Header("Agent Parameters")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    private CharacterController enemyMovement;

    [Header("Environment References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform safeZone;
    [SerializeField] private float detectionRange = 10f;

    private Vector3 startingPosition;

    public override void Initialize()
    {
        enemyMovement = GetComponent<CharacterController>();
        startingPosition = transform.localPosition;
    }

    public override void OnEpisodeBegin()
    {
        // Ajanın pozisyonunu sıfırla
        transform.localPosition = startingPosition;

        // Oyuncunun pozisyonunu rastgele belirle (örnek için)
        player.localPosition = new Vector3(UnityEngine.Random.Range(-5f, 5f), 0f, UnityEngine.Random.Range(-5f, 5f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ajanın ve oyuncunun pozisyonlarını gözlemle
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(player.localPosition);

        // Ajanın oyuncuya olan uzaklığını gözlemle
        sensor.AddObservation(Vector3.Distance(transform.localPosition, player.localPosition));
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Sürekli hareket (ileri-geri ve sağa-sola)
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        // Dönme hareketi
        float rotate = actions.ContinuousActions[2];

        // Hareketi uygula
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized * moveSpeed * Time.deltaTime;
        enemyMovement.Move(move);
        transform.Rotate(0, rotate * rotationSpeed * Time.deltaTime, 0);

        // Oyuncudan uzaklaşmayı ödüllendir
        float distanceToPlayer = Vector3.Distance(transform.localPosition, player.localPosition);
        if (distanceToPlayer > detectionRange)
        {
            AddReward(0.1f); // Oyuncudan uzakta kalmayı ödüllendir
        }
        else
        {
            AddReward(-0.1f); // Oyuncuya yakın kalmayı cezalandır
        }

        // Güvenli bölgeye yaklaşmayı ödüllendir
        float distanceToSafeZone = Vector3.Distance(transform.localPosition, safeZone.localPosition);
        if (distanceToSafeZone < 2f)
        {
            AddReward(1f); // Güvenli bölgeye ulaşmayı ödüllendir
            EndEpisode();  // Bölümü bitir
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Manuel kontrol için sürekli hareketleri tanımla
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
        continuousActions[2] = Input.GetAxis("Mouse X");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Eğer ajan oyuncu tarafından vurulursa
        if (collision.collider.CompareTag("Weapon"))
        {
            AddReward(-1f); // Ceza ver
            EndEpisode();   // Bölümü bitir
        }
    }
}
