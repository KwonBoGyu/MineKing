using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SCENE
{
    TITLE,
    MAIN,
    SKIN,
    DUNGEON_FIRST_1,
    DUNGEON_FIRST_2,
    DUNGEON_FIRST_3,
    DUNGEON_FIRST_4,
    DUNGEON_FIRST_5,
    DUNGEON_FIRST_6,
    DUNGEON_FIRST_7,
    DUNGEON_FIRST_8,
    DUNGEON_FIRST_9,
    DUNGEON_FIRST_10,
    DUNGEON_FIRST_11,
    DUNGEON_FIRST_12,
    DUNGEON_FIRST_13,
    DUNGEON_FIRST_14,
    DUNGEON_FIRST_15,
    DUNGEON_FIRST_BOSS_1,
    DUNGEON_FIRST_BOSS_2,
    DUNGEON_FIRST_BOSS_3,
    DUNGEON_FIRST_BOSS_4,
    DUNGEON_FIRST_BOSS_5,    
}

public class cSceneManager : MonoBehaviour
{
    //페이드인아웃 관련 변수
    public Image img_Fade;    
    private float alpha;    
    //씬 관련 변수
    public SCENE _scene;    
    public GameObject _loadingImg;
    public GameObject clickBarrier;

    private bool _isStarted;

    private void Start()
    {
        alpha = 0.0f;
        _scene = SCENE.TITLE;
        _isStarted = false;
        clickBarrier.SetActive(false);
        Init();
    }
    
    private void Init()
    {
        _isStarted = true;
    }

    //씬 변경
    public void ChangeScene(string pSceneName)
    {
        int sceneNum = 0;
        switch(pSceneName)
        {
            case "Title": sceneNum = 0; break;
            case "Main": sceneNum = 1; break;
            case "Main_skin": sceneNum = 2; break;
            case "Dungeon_normal_1": sceneNum = 3; break;
            case "Dungeon_normal_2": sceneNum = 4; break;
            case "Dungeon_normal_3": sceneNum = 5; break;
            case "Dungeon_normal_4": sceneNum = 6; break;
            case "Dungeon_normal_5": sceneNum = 7; break;
            case "Dungeon_normal_6": sceneNum = 8; break;
            case "Dungeon_normal_7": sceneNum = 9; break;
            case "Dungeon_normal_8": sceneNum = 10; break;
            case "Dungeon_normal_9": sceneNum = 11; break;
            case "Dungeon_normal_10": sceneNum = 12; break;
            case "Dungeon_normal_11": sceneNum = 13; break;
            case "Dungeon_normal_12": sceneNum = 14; break;
            case "Dungeon_normal_13": sceneNum = 15; break;
            case "Dungeon_normal_14": sceneNum = 16; break;
            case "Dungeon_normal_15": sceneNum = 17; break;
            case "Dungeon_normal_boss_1": sceneNum = 18; break;
            case "Dungeon_normal_boss_2": sceneNum = 19; break;
            case "Dungeon_normal_boss_3": sceneNum = 20; break;
            case "Dungeon_normal_boss_4": sceneNum = 21; break;
            case "Dungeon_normal_boss_5": sceneNum = 22; break;
        }

        //이미 같은 씬일 때 리턴
        if(sceneNum == (int)_scene)
        {
            Debug.Log("이미 같은 씬입니다. -" + pSceneName);
            return;
        }

        //같은 씬이 아니라면 씬 전환 
        clickBarrier.SetActive(true);
        _scene = (SCENE)sceneNum;
        StartCoroutine(LoadScene(pSceneName));
    }
    
    public void GoToStageGate(bool pIsNextDoor)
    {
        int sceneNum = (int)_scene;

        //다음 스테이지로
        if(pIsNextDoor.Equals(true))
        {
            if (sceneNum < 18)
            {
                    sceneNum += 1;
            }
            else
                return;
        }
        //이전 스테이지로
        else
        {
            if (sceneNum > 3)
            {
                sceneNum -= 1;
            }
            else
                return;
        }

        _scene = (SCENE)sceneNum;
        StartCoroutine(LoadScene("Dungeon_normal_" + (sceneNum - 2).ToString()));
    }
    public void GoToBossGate(bool pIsNextDoor)
    {
        int sceneNum = (int)_scene;

        //보스 스테이지로
        if (pIsNextDoor.Equals(true))
        {
            if(sceneNum.Equals(5))
                sceneNum = 18;
            else if (sceneNum.Equals(8))
                sceneNum = 19;
            else if (sceneNum.Equals(11))
                sceneNum = 20;
            else if (sceneNum.Equals(14))
                sceneNum = 21;
            else if (sceneNum.Equals(17))
                sceneNum = 22;

            _scene = (SCENE)sceneNum;
            StartCoroutine(LoadScene("Dungeon_boss_" + (sceneNum - 17).ToString()));
        }
        //필드 스테이지로
        else
        {
            if (sceneNum.Equals(18))
                sceneNum = 5;
            else if (sceneNum.Equals(19))
                sceneNum = 8;
            else if (sceneNum.Equals(20))
                sceneNum = 11;
            else if (sceneNum.Equals(21))
                sceneNum = 14;
            else if (sceneNum.Equals(22))
                sceneNum = 17;

            _scene = (SCENE)sceneNum;
            StartCoroutine(LoadScene("Dungeon_normal_" + (sceneNum - 2).ToString()));
        }
    }

    //씬 로딩 코루틴
    IEnumerator LoadScene(string pSceneName)
    {
        //페이드 아웃 끝날 때 까지 대기
        yield return StartCoroutine(FadeOut());

        //로딩 씬
        AsyncOperation op = SceneManager.LoadSceneAsync(pSceneName);
        op.allowSceneActivation = false;
        _loadingImg.SetActive(true);
        Image _progressBar = _loadingImg.transform.Find("progressBar").GetComponent<Image>();

        while (op.isDone == false)
        {
            yield return null;
            _progressBar.fillAmount = op.progress;

            //로딩 완료 됐다면..
            if (op.progress >= 0.9f)
            {
                _progressBar.fillAmount = 1.0f;
                op.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(1.0f);
            _progressBar.fillAmount = 0.0f;
            _loadingImg.SetActive(false);
        }

        SceneManager.LoadScene(pSceneName);

        //페이드 인
        yield return StartCoroutine(FadeIn());
    }

    #region 페이드 인아웃 함수	
    IEnumerator FadeOut()
    {   
        while(alpha < 1.0f)
        {
            yield return null;

            alpha += 1.0f * Time.deltaTime;
            if (alpha >= 1.0f)
                alpha = 1.0f;

            img_Fade.color = new Color(img_Fade.color.r, img_Fade.color.g, img_Fade.color.b, alpha);
        }

        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator FadeIn()
    {
        while (alpha > 0.0f)
        {
            yield return null;

            alpha -= 1.0f * Time.deltaTime;
            if (alpha <= 0)
                alpha = 0.0f;

            img_Fade.color = new Color(img_Fade.color.r, img_Fade.color.g, img_Fade.color.b, alpha);
        }
        clickBarrier.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }
    #endregion
}
