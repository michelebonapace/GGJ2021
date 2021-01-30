using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveLuggage : Luggage
{
    public float heightOffset = 0.2f;
    public float explosionStrenght = 5f;
    public float explosionRadius = 5f;
    public float explosionUpForce = 1f;

    PlayerController player;
    PlayerController Player { get { if (player == null) player = FindObjectOfType<PlayerController>(); return player; } }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player.gameObject && Player.transform.position.y > transform.position.y + heightOffset)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            Player.StunPlayer();

            foreach (Collider hitObject in colliders)
            {
                Rigidbody rigidBody = hitObject.GetComponent<Rigidbody>();

                if (rigidBody != null && !rigidBody.isKinematic)
                {
                    rigidBody.AddExplosionForce(explosionStrenght, transform.position, explosionRadius, explosionUpForce, ForceMode.Impulse);
                }
            }
        }
    }
}
