using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Round
{
    public Round()
    {

    }
}

public class RoundManager : MonoBehaviour
{
    public static RoundManager instance;
    public List<Round> rounds;
    public int currentRound = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        rounds = new List<Round>();
    }

    public void AddCurrentRoundProgress()
    {
        Round round = new Round();
        rounds.Add(round);
    }

    public void ClearAllRounds()
    {
        rounds.Clear();
    }
}
