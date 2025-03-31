using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    private Rigidbody2D _rigidBody;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void ShootAt(Vector2 direction, float speed = 100)
    {
        gameObject.SetActive(true);
        Invoke(nameof(Deactivate), lifeTime);

        _rigidBody.velocity = direction * speed;
        // Debug.Log("Bullet travels in " + direction + " with speed " + _rigidBody.velocity);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            // Debug.Log("Bullet hit wall at " + transform.position);
            Deactivate();
            CancelInvoke();
        }
    }
}