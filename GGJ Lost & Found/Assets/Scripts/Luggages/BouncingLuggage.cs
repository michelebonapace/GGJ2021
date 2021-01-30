using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingLuggage : Luggage
{
    public float heightOffset = 0.2f;
    public float bounceStrenght = 4f;

    PlayerController player;
    PlayerController Player { get { if (player == null) player = Gamemanager.Instance.Player; return player; } }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player.gameObject && Player.transform.position.y > transform.position.y + heightOffset)
        {
            Player.Body.AddForce(Vector3.up * bounceStrenght, ForceMode.Impulse);
        }
    }
}
