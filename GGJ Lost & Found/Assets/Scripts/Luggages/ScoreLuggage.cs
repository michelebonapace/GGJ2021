using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreLuggage : MonoBehaviour
{
    public LayerMask playerLayer;
    public string playerTag;
    public int score;
    public int time;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag =="Player")
        {
            Gamemanager.Instance.AddScore(score);
            Gamemanager.Instance.AddTime(time);
            Gamemanager.Instance.Player.Scream();

            SectionsManager.Instance.ReplaceLuggage(transform);
        }
    }
}
