using DesignPatterns.Generics;
using System;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    

    [SerializeField] GameObject loaderCanvas;
    [SerializeField] Image progressBar;
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeSpeed;
    [SerializeField] string[] userTips;
    [SerializeField] TextMeshProUGUI userTipText;

    public override void Awake()
    {
        base.Awake();
    }
    public void ChangeScene(string _sceneName)
    {
        StartCoroutine(PreloadFadeIn(
            () => StartCoroutine(LoadSceneAsync(_sceneName, LoadSceneMode.Single,
            () =>
            {
                loaderCanvas.SetActive(false);
            },
            () =>
            {
                StartCoroutine(PostLoadFadeOut(null));
            }
            ))));
    }
    public void ChangeSceneWithLoading(string _sceneName)
    {
        StartCoroutine(PreloadFadeIn(
            () => StartCoroutine(LoadSceneAsync(_sceneName, LoadSceneMode.Single,
            ()=>
            {
                loaderCanvas.SetActive(true);
                progressBar.fillAmount = 0;

                //esempio in cui metto un consiglio nel gioco
                userTipText.text = userTips[UnityEngine.Random.Range(0, userTips.Count())];
            },
            () =>
            {
                loaderCanvas.SetActive(false);
                progressBar.fillAmount = 0;
                StartCoroutine(PostLoadFadeOut(null));
            }
            ))));
    }
    public void AddScene(string _sceneName)
    {
       StartCoroutine(LoadSceneAsync(_sceneName, LoadSceneMode.Additive,
            () =>
            {
                loaderCanvas.SetActive(false);
            },
            () =>
            {
                
            }
            ));
    }

    private IEnumerator PreloadFadeIn(Action onPreloadEnd)
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0);
        while (true)
        {
            yield return new WaitForEndOfFrame();
            fadePanel.color = new Color(
                fadePanel.color.r, 
                fadePanel.color.g,
                fadePanel.color.b, 
                Time.deltaTime*fadeSpeed+ fadePanel.color.a);

            if(fadePanel.color.a >= 1)
            {
                fadePanel.color = new Color(
                fadePanel.color.r,
                fadePanel.color.g,
                fadePanel.color.b,
                1);

                break;
            }
        }

        onPreloadEnd?.Invoke();
    }

    private IEnumerator LoadSceneAsync(string _sceneName, LoadSceneMode _loadMode, Action onLoadSceneStart, Action onLoadSceneEnd)
    {

        onLoadSceneStart?.Invoke();

        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneName, _loadMode); //con LoadSceneMode.Additive aggounge una scena
        //operation.allowSceneActivation = false;  gestisco io e faccio in modo che la scena non sia visibile fino a quando voglio io
        while(!operation.isDone)
        {
            progressBar.fillAmount = Mathf.Clamp01(operation.progress / 0.9f);
            yield return new WaitForEndOfFrame();

            //if (progressBar.fillAmount == 1)
            //{
            //    //aggiungere delay per dare più una sensazione di ritardo
            //    yield return new WaitForSeconds(tot secondi che volete);

            //    operation.allowSceneActivation = true;
            //}
        }

        onLoadSceneEnd?.Invoke();

        //yield return StartCoroutine(PostLoadFadeOut(onPostLoadEnd));
    }
    private IEnumerator PostLoadFadeOut(Action _onPostLoadEnd)
    {
        fadePanel.gameObject.SetActive(true);
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1);
        while (true)
        {
            yield return new WaitForEndOfFrame();
            fadePanel.color = new Color(
                fadePanel.color.r,
                fadePanel.color.g,
                fadePanel.color.b,
                fadePanel.color.a - Time.deltaTime * fadeSpeed);

            if (fadePanel.color.a <= 0)
            {
                fadePanel.color = new Color(
                fadePanel.color.r,
                fadePanel.color.g,
                fadePanel.color.b,
                0);

                break;
            }
        }
        fadePanel.gameObject.SetActive(false);

        _onPostLoadEnd?.Invoke();
    }
}
