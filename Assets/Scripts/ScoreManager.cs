using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    private long currentScore = 0;

    // This method is just a placeholder. You'd have some method(s) to update this score.
    public void AddToScore(long value)
    {
        currentScore += value;
    }

    public long GetScore()
    {
        return currentScore;
    }
}

