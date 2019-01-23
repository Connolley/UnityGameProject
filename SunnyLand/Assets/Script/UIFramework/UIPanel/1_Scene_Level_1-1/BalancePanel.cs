﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class BalancePanel : BasePanel
{
    private Text score;
    private Text reward;
    private Image[] star = new Image[3];

    protected override void Awake()
    {
        base.Awake();
        score = transform.Find("ScorePanel/Score").GetComponent<Text>();
        reward = transform.Find("RewardPanel/Reward").GetComponent<Text>();

        for (int i = 0;i<star.Length;i++)
        {
            star[i] = transform.Find("StarPanel/Star" + ( i + 1 ) + "/star").GetComponent<Image>();
            star[i].gameObject.SetActive(false);
        }

        ctrl.model.OnBalance += OnBalance; 
    }

    private void OnBalance(int score,int specialCoin)
    {
        StartCoroutine(ScoreAnim(score,this.score));
        StartCoroutine(ScoreAnim(score * 3, reward));
        StartCoroutine(StarAnim(specialCoin));

        //for (int i = 0; i < star.Length; i++)
        //{
        //    if (i < specialCoin)
        //    {
        //        star[i].gameObject.SetActive(true);
        //        star[i].transform.DOScale(Vector3.one, 0.2f);
        //    }
        //    else
        //        break;
        //}
    }


    private IEnumerator ScoreAnim(int score,Text text)
    {
        for (int i = 0; i < score + 1; i += 10)
        {
            text.text = i.ToString();
            //ctrl.audioManager.Play(ctrl.audioManager.GetScore, ctrl.source);

            yield return new WaitForSeconds(0.01f);
        }

        StopCoroutine(ScoreAnim(score, text));
    }

    private IEnumerator StarAnim(int specialCoin)
    {
        for (int i = 0; i < star.Length; i++)
        {
            if (i < specialCoin)
            {
                star[i].gameObject.SetActive(true);
                star[i].transform.DOScale(Vector3.one, 0.2f);
                ctrl.audioManager.Play(ctrl.audioManager.GetStar, ctrl.source);

                yield return new WaitForSeconds(0.2f);
            }
            else
                break;
        }

        StopCoroutine(StarAnim(specialCoin));
    }

    #region button click event

    public void OnClickReturnButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnClickRestartButton()
    {
        ctrl.model.levelIndex--;
        SceneManager.LoadScene(ctrl.model.levelIndex + 1);
    }

    public void OnClickNextLevelButton()
    {
        SceneManager.LoadScene(ctrl.model.levelIndex + 1);        
    }

    #endregion
}
