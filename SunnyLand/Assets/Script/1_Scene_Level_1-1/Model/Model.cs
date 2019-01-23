﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Model : MonoBehaviour
{
    public GameRecordInfo gameRecord;
    public event Action<int> OnScoreChange;
    public event Action<int> OnMaxScoreChange;
    public event Action<int, int> OnBalance;

    private int levelIndex = 0;
    private int score = 0;
    private int maxScore = 0;
    private int specialCoin = 0;

    private void Awake()
    {
        levelIndex = gameRecord.levelIndex;

        LoadScore();  
    }

    private void Update()
    {
        if (maxScore < score)
            maxScore = score;

        if (OnMaxScoreChange != null)
            OnMaxScoreChange(maxScore);

        if (OnScoreChange != null)
            OnScoreChange(score);
    }

    private void LoadScore()
    {
        if (levelIndex >= gameRecord.beingPassedLevel)
        {
            if (gameRecord.maxScores.Count < levelIndex)
                gameRecord.maxScores.Add(maxScore);
            if (gameRecord.specialRewards.Count < levelIndex)
                gameRecord.specialRewards.Add(specialCoin);
        }
        else
        {
            maxScore = gameRecord.maxScores[levelIndex - 1];
        }
    }

    public void GetScore(string tag)
    {
        switch (tag)
        {
            case "CommonCoin":
                {
                    score += 20;
                }
                break;
            case "SpecialCoin":
                {
                    score += 100;
                    specialCoin++;
                }
                break;
            case "CommonEnemy":
                {
                    score += 50;
                }
                break;
            case "SpecialEnemy":
                {
                    score += 100;
                }
                break;
            default: break;
        }
    }

    public void SaveScore()
    {
        if(OnBalance != null)
            OnBalance(score,specialCoin);

        if(gameRecord.maxScores[levelIndex - 1] < maxScore)
        gameRecord.maxScores[levelIndex - 1] = maxScore;

        if (gameRecord.specialRewards[levelIndex - 1] < specialCoin)
            gameRecord.specialRewards[levelIndex - 1] = specialCoin;

        if(levelIndex == gameRecord.beingPassedLevel)
            gameRecord.beingPassedLevel++;

        gameRecord.Save();
    }   
}
