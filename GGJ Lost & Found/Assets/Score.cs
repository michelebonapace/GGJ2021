using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        if (score != 0)
        {
            this.GetComponent<Text>().text = "" + score;
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

}
