using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class EnemyAgentController : Agent
{
    [Header("Character Parameters")]
    [SerializeField] private float moveSpeed = 2f;
    private CharacterController enemyMovement;
    [SerializeField] private GunController gunObject;
    private bool can_shoot, hit_target, has_shot = false;
    private int time_until_next_bullet = 0;
    private int min_time_until_next_bullet = 120;
    private Animator _animator;
    private bool isMoving;

    // Yerçekimi parametreleri
    private Vector3 velocity;
    [SerializeField] private float gravity = -9.81f; // Yerçekimi kuvveti
    [SerializeField] private float groundCheckDistance = 0.1f; // Yere yakınlık kontrolü
    [SerializeField] private LayerMask groundLayer; // Zemin katmanı
    private bool isGrounded;

    public override void Initialize()
    {
        _animator = GetComponent<Animator>();
        enemyMovement = GetComponent<CharacterController>();
    }

    public override void OnEpisodeBegin()
    {
        velocity = Vector3.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        can_shoot = false;

        float move_Rotate = actions.ContinuousActions[0];
        float move_Forward = actions.ContinuousActions[1];
        bool shoot = actions.DiscreteActions[0] > 0;

        // Hareket
        Vector3 moveDirection = transform.forward * (move_Forward * moveSpeed * Time.deltaTime);
        enemyMovement.Move(moveDirection);

        // Dönüş
        transform.Rotate(0f, move_Rotate, 0f, Space.Self);

        // Ateş Etme
        if (shoot && !has_shot)
        {
            can_shoot = true;
        }

        if (can_shoot)
        {
            hit_target = gunObject.ShootGun();

            time_until_next_bullet = min_time_until_next_bullet;
            has_shot = true;
            if (hit_target)
            {
                AddReward(30);
            }
            else
            {
                AddReward(-1);
            }
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousAction = actionsOut.ContinuousActions;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        continuousAction[0] = Input.GetAxis("Horizontal");
        continuousAction[1] = Input.GetAxis("Vertical");
        discreteActions[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }

    private void FixedUpdate()
    {
        if (has_shot)
        {
            time_until_next_bullet--;
            if (time_until_next_bullet <= 0)
            {
                has_shot = false;
            }
        }

        // Yere temas kontrolü
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Zemin üzerinde sabit kalması için hafif bir negatif değer
        }

        // Yerçekimi uygulama
        velocity.y += gravity * Time.fixedDeltaTime;
        enemyMovement.Move(velocity * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (enemyMovement.velocity != Vector3.zero)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Wall"))
        {
            AddReward(-15f);
        }
        else if (hit.collider.CompareTag("Player"))
        {
            AddReward(-15f);
        }
    }

    private void OnAnimatorMove()
    {
        _animator.SetBool("canMove", isMoving);
    }
}
