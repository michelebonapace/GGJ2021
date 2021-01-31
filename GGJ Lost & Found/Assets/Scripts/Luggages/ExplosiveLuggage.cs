using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ExplosiveLuggage : Luggage
{
    [SerializeField] private VisualEffect explosionPrefab = null;

    [SerializeField] private float heightOffset = 0.2f;
    [SerializeField] private float explosionStrenght = 5f;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionUpForce = 1f;

    private static VisualEffect explosionParticle = null;

    Player player;
    Player Player { get { if (player == null) player = Gamemanager.Instance.Player; return player; } }

    private void Start()
    {
        if (explosionParticle == null)
        {
            explosionParticle = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player.gameObject /*&& Player.transform.position.y > transform.position.y + heightOffset*/)
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

            explosionParticle.transform.position = transform.position;
            explosionParticle.Play();

            SectionsManager.Instance.ReplaceLuggage(this.transform);
        }
    }
}
