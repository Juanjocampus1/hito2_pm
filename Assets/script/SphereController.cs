using UnityEngine;

public class SphereController : MonoBehaviour
{
    private int bounceCount = 0;
    private int maxBounces = 3;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Desactivar el enemigo
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Hacer daño al jugador
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }

        bounceCount++;
        if (bounceCount >= maxBounces)
        {
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        bounceCount = 0;
        gameObject.SetActive(false);
        // Aquí puedes llamar a un método en PrefabPool para devolver el objeto a la pool si es necesario
    }
}

