using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // Start is called before the first frame update
    public int score;

    private void Awake()
    {
        if (ScoreManager.Instance == null)
        {
            ScoreManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy (gameObject);
        }
    }

    void Start()
    {
        score = 0;
    }

    public void addScore(int n)
    {
        score += n;
    }

    public void resScore(int n)
    {
        score -= n;
    }
}
