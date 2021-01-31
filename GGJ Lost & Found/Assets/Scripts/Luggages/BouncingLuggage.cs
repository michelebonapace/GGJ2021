using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingLuggage : Luggage
{
    [SerializeField] private AudioClip[] bounceClips = null;

    public float heightOffset = 0.2f;
    public float bounceStrenght = 4f;

    private AudioSource audioSource;

    Player player;
    Player Player { get { if (player == null) player = Gamemanager.Instance.Player; return player; } }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player.gameObject && Player.transform.position.y > transform.position.y + heightOffset)
        {
            Player.Body.AddForce(Vector3.up * bounceStrenght, ForceMode.Impulse);
            audioSource.PlayOneShot(bounceClips[Random.Range(0, bounceClips.Length)]);
        }
    }
}
