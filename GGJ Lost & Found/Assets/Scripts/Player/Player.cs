using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody body;
    public Rigidbody Body { get => body; }

    public virtual void StunPlayer() { }
    public virtual void Scream() { }
}
