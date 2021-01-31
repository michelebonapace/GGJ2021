using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreLuggage : MonoBehaviour{

    public LayerMask playerLayer;
    public string playerTag;
    public int score;

    private void OnCollisionEnter(Collision collision) {

        if ((playerLayer.value & 1 << collision.gameObject.layer) != 0 || collision.gameObject.tag.CompareTo(playerTag) == 0) {
            Gamemanager.Instance.AddScore(score);
            //TODO: destroy o boh
        }
    }
}
