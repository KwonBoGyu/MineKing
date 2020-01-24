using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum SCENE
{
    TITLE,
    MAIN,
    DUNGEON_FIRST_1,
    DUNGEON_FIRST_BOSS_1,
    DUNGEON_FIRST_2,
    DUNGEON_FIRST_BOSS_2,
    DUNGEON_FIRST_3,
    DUNGEON_FIRST_BOSS_3,
    DUNGEON_FIRST_4,
    DUNGEON_FIRST_BOSS_4,
    DUNGEON_FIRST_5,
    DUNGEON_FIRST_BOSS_5,
    DUNGEON_SECOND_1,
    DUNGEON_SECOND_BOSS_1,
    DUNGEON_SECOND_2,
    DUNGEON_SECOND_BOSS_2,
    DUNGEON_SECOND_3,
    DUNGEON_SECOND_BOSS_3,
    DUNGEON_SECOND_4,
    DUNGEON_SECOND_BOSS_4,
    DUNGEON_SECOND_5,
    DUNGEON_SECOND_BOSS_5,
    DUNGEON_THIRD_1,
    DUNGEON_THIRD_BOSS_1,
    DUNGEON_THIRD_2,
    DUNGEON_THIRD_BOSS_2,
    DUNGEON_THIRD_3,
    DUNGEON_THIRD_BOSS_3,
    DUNGEON_THIRD_4,
    DUNGEON_THIRD_BOSS_4,
    DUNGEON_THIRD_5,
    DUNGEON_THIRD_BOSS_5,
    DUNGEON_FOURTH_1,
    DUNGEON_FOURTH_BOSS_1,
    DUNGEON_FOURTH_2,
    DUNGEON_FOURTH_BOSS_2,
    DUNGEON_FOURTH_3,
    DUNGEON_FOURTH_BOSS_3,
    DUNGEON_FOURTH_4,
    DUNGEON_FOURTH_BOSS_4,
    DUNGEON_FOURTH_5,
    DUNGEON_FOURTH_BOSS_5,
}

public class cSceneManager : MonoBehaviour
{
    //페이드인아웃 관련 변수
    public Image img_Fade;    
    private float alpha;    
    //씬 관련 변수
    public SCENE _scene;    
    public GameObject _loadingImg;

    private bool _isStarted;

    private void Start()
    {
        alpha = 0.0f;
        _scene = SCENE.TITLE;
        _isStarted = false;

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
            case "Dungeon_normal": sceneNum = 2; break;
            case "Dungeon_boss": sceneNum = 3; break;
        }

        //이미 같은 씬일 때 리턴
        if(sceneNum == (int)_scene)
        {
            Debug.Log("이미 같은 씬입니다. -" + pSceneName);
            return;
        }

        //같은 씬이 아니라면 씬 전환 
        _scene = (SCENE)sceneNum;
        StartCoroutine(LoadScene(pSceneName));
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

        yield return new WaitForSeconds(0.5f);
    }
    #endregion
}
