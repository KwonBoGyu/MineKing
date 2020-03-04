using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cCamera : MonoBehaviour
{
    public Transform trans_nextGate;
    private bool stopPlayerCam;

    public GameObject _player;
    public Camera cam;
    public Image canvas;
    public Image moveArea;
    private RectTransform camRt;

    private Vector4 camRange; // 현재 카메라가 비추는 범위
    private Vector4 minMax_entireRange; // 맵 전체 범위
    private Vector4 minMax_moveRange; // 카메라 고정 범위

    private float moveRange_width;
    private float moveRange_height;

    // x,y축 잠금 여부
    bool isXLocked;
    bool isYLocked;

    // 최종 카메라 x,y 포지션
    float xFixedPos;
    float yFixedPos;

    float cam_height;
    float cam_width;

    void Start()
    {
        RectTransform bt = canvas.GetComponent<RectTransform>();
        minMax_entireRange = new Vector4();
        minMax_entireRange.x = bt.position.y + bt.sizeDelta.y * 0.5f; 
        minMax_entireRange.y = bt.position.x + bt.sizeDelta.x * 0.5f;
        minMax_entireRange.z = bt.position.y - bt.sizeDelta.y * 0.5f;
        minMax_entireRange.w = bt.position.x - bt.sizeDelta.x * 0.5f;

        RectTransform mv = moveArea.GetComponent<RectTransform>();
        moveRange_width = mv.sizeDelta.x;
        moveRange_height = mv.sizeDelta.y;
        
        camRt = this.GetComponent<RectTransform>();

        cam_height = 2 * cam.orthographicSize;
        cam_width = cam_height * cam.aspect;
        
    }

    void FixedUpdate()
    {
        if(stopPlayerCam.Equals(false))
        {
            // 카메라 경계 실시간 위치값
            camRange.x = cam.ViewportToWorldPoint(new Vector2(0, 1)).y;
            camRange.y = cam.ViewportToWorldPoint(new Vector2(1, 1)).x;
            camRange.z = cam.ViewportToWorldPoint(new Vector2(1, 0)).y;
            camRange.w = cam.ViewportToWorldPoint(new Vector2(0, 0)).x;

            //전체 맵 바운딩
            if (camRange.x >= minMax_entireRange.x)
            {
                yFixedPos = minMax_entireRange.x - cam_height * 0.5f;

                isYLocked = true;
            }
            if (camRange.y >= minMax_entireRange.y)
            {
                xFixedPos = minMax_entireRange.y - cam_width * 0.5f;

                isXLocked = true;
                //Debug.Log("오른");
            }
            if (camRange.z <= minMax_entireRange.z)
            {
                yFixedPos = minMax_entireRange.z + cam_height * 0.5f;

                isYLocked = true;
                //Debug.Log("아래");
            }
            if (camRange.w <= minMax_entireRange.w)
            {
                xFixedPos = minMax_entireRange.w + cam_width * 0.5f;

                isXLocked = true;
                //Debug.Log("왼");
            }

            // 가로 방향 바운더리에서 캐릭터가 벗어난 경우
            if ((_player.transform.position.x > minMax_entireRange.w + cam_width / 2 + moveRange_width * 0.5f) &&
                    (_player.transform.position.x < minMax_entireRange.y - cam_width / 2 - moveRange_height * 0.5f))
            {
                isXLocked = false;
            }
            // 세로 방향 바운더리에서 캐릭터가 벗어난 경우
            if ((_player.transform.position.y > minMax_entireRange.z + cam_height / 2) &&
                    (_player.transform.position.y < minMax_entireRange.x - cam_height / 2))
            {
                isYLocked = false;
            }

            // 플레이어 팔로우
            // X방향만 잠긴 경우
            if (isXLocked.Equals(true) && isYLocked.Equals(false))
            {
                // 카메라 고정 범위 밖으로 플레이어 이동
                if (Vector3.Distance(_player.transform.position, moveArea.transform.position) > moveRange_width * 0.5f)
                {
                    yFixedPos = Mathf.Lerp(this.gameObject.transform.position.y, _player.transform.position.y, 0.2f);
                }
                // 이동 범위 안인 경우 고정
                else
                {
                    yFixedPos = Mathf.Lerp(this.transform.position.y, _player.transform.position.y, 0.2f);
                }
            }
            // Y방향만 잠긴 경우
            else if (isXLocked.Equals(false) && isYLocked.Equals(true))
            {
                // 카메라 고정 범위 밖으로 플레이어 이동
                if (Vector3.Distance(_player.transform.position, moveArea.transform.position) > moveRange_width * 0.5f)
                {
                    // 오른쪽으로 플레이어 이동중인 경우
                    if (_player.transform.position.x >= this.transform.position.x)
                    {
                        xFixedPos = Mathf.Lerp(this.transform.position.x, _player.transform.position.x - moveRange_width * 0.5f, 0.2f);
                    }
                    // 왼쪽으로 플레이어 이동중인 경우
                    else
                    {
                        xFixedPos = Mathf.Lerp(this.transform.position.x, _player.transform.position.x + moveRange_width * 0.5f, 0.2f);
                    }
                }
                // 이동 범위 안인 경우
                else
                {
                    xFixedPos = this.gameObject.transform.position.x;
                }
            }
            // 두 방향 모두 잠기지 않은 경우
            else if (isXLocked.Equals(false) && isYLocked.Equals(false))
            {
                // 카메라 고정 범위 밖으로 플레이어 이동
                if (Vector2.Distance(_player.transform.position, moveArea.transform.position) > moveRange_width * 0.5f)
                {
                    // 오른쪽
                    if (_player.transform.position.x >= this.transform.position.x)
                    {
                        xFixedPos = Mathf.Lerp(this.transform.position.x, _player.transform.position.x - moveRange_width * 0.5f, 0.2f);
                    }
                    // 왼쪽
                    else
                    {
                        xFixedPos = Mathf.Lerp(this.transform.position.x, _player.transform.position.x + moveRange_width * 0.5f, 0.2f);
                    }
                    yFixedPos = Mathf.Lerp(this.transform.position.y, _player.transform.position.y, 0.2f);
                }
                // 이동 범위 안
                else
                {
                    xFixedPos = this.gameObject.transform.position.x;
                    yFixedPos = Mathf.Lerp(this.transform.position.y, _player.transform.position.y, 0.2f);
                }
            }
            this.gameObject.transform.position = new Vector3(xFixedPos, yFixedPos, -9.5f);
        }
        
    }

    public void ShowNextGate()
    {
        StartCoroutine(Cor_ShowNextGate());
    }

    IEnumerator Cor_ShowNextGate()
    {
        yield return new WaitForSeconds(5.0f);

        stopPlayerCam = true;
        Vector2 dir = (trans_nextGate.position - this.transform.position).normalized;

        float speed = 2000;

        while(true)
        {
            yield return new WaitForFixedUpdate();

            float dist = Vector2.Distance(this.transform.position, trans_nextGate.position);
            Debug.Log(dist);
            //도착하였다면..
            if (dist < 50)
            {
                trans_nextGate.GetComponent<Animator>().SetTrigger("OpenGate");
                yield return new WaitForSeconds(2.0f);

                trans_nextGate.GetComponent<Button>().interactable = true;
                stopPlayerCam = false;
                break;
            }

            this.transform.Translate(dir * speed * Time.deltaTime);
        }
    }
}