using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 5f;
    public float speed = 2f;
    public float wanderRadius = 5f;
    public float wanderTimer = 3f;
    public PrefabPool prefabPool; // Referencia a la pool de prefabs
    public float shootForce = 10f; // Fuerza inicial del disparo
    public float shootCooldown = 2f; // Tiempo de espera entre disparos

    private float timer;
    private float shootTimer;
    private Vector3 wanderTarget;

    // Start is called before the first frame update
    void Start()
    {
        timer = wanderTimer;
        shootTimer = shootCooldown;
        wanderTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > attackRange)
            {
                // Moverse hacia el jugador y disparar esferas
                MoveTowardsPlayer();
                LookAtPlayer();
                if (shootTimer >= shootCooldown)
                {
                    LaunchSphere();
                    shootTimer = 0f;
                }
            }
            else
            {
                // Chocarse con el jugador
                ChargePlayer();
            }
        }
        else
        {
            // Deambular
            Wander();
        }

        shootTimer += Time.deltaTime;
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void ChargePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime * 2; // Doble de velocidad para chocar
    }

    void LaunchSphere()
    {
        // Pedir una esfera a la pool
        GameObject sphere = prefabPool.GetPoolObject();
        if (sphere != null)
        {
            // Posicionar la esfera desactivada en la posición del enemigo
            sphere.transform.position = transform.position + transform.forward;
            sphere.transform.rotation = transform.rotation;

            // Activar la esfera
            sphere.SetActive(true);

            // Acceder al rigidbody de la esfera y aplicarle una fuerza hacia adelante
            Rigidbody rb = sphere.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = transform.forward * shootForce;
            }
        }
        else
        {
            Debug.Log("No hay esferas disponibles en la pool");
        }

        Debug.Log("Lanzando esfera");
    }

    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            wanderTarget = newPos;
            timer = 0;
        }

        Vector3 direction = (wanderTarget - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hacer que el jugador muera instantáneamente
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Die();
            }
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        return new Vector3(randDirection.x, origin.y, randDirection.z);
    }
}

