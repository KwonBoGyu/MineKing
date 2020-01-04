//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class cSwipeMng : MonoBehaviour
//{
//    private Vector2 ScreenSize;
//    private float minSwipeDist;
//    private Vector2 swipeDir;
//    private bool isSwiped;
//    private float curSwipeTimer;
//    private float maxSwipeTime;
//    private float prevMousePos;
    
//    Touch tempTouch;

//    void Start()
//    {
//        ScreenSize = new Vector2(Screen.width, Screen.height);
//        minSwipeDist = ScreenSize.x * 0.2f;
//        Debug.Log(minSwipeDist);
//        maxSwipeTime = 0.7f;
//    }

//    void Update()
//    {
//#if ANDROID
//        Swipe();
//#else
//        Swipe_unity();
//#endif
//    }

//    private void Swipe()
//    {
//        if(Input.touches.Length > 0)
//        {
//            //왼쪽 화면에서 터치했을 때 해당 터치 정보 가져온.
//            for (int i = 0; i < Input.touches.Length; i++)
//            {
//                if(Input.GetTouch(i).position.x < (ScreenSize.x * 0.5f))
//                {
//                    tempTouch = Input.GetTouch(i);
//                    break;
//                }
//            }

//            Debug.Log(tempTouch.position);
//        }
//    }

//    private void Swipe_unity()
//    {
//        `

//        if(Input.GetMouseButtonDown(0).Equals(true))
//        {
//            prevMousePos = Input.mousePosition;
//            isSwiped = false;
//            curSwipeTimer = 0;
//        }
//        else if(Input.GetMouseButton(0).Equals(true))
//        {
//            Debug.Log("swipe Doing");
//            curSwipeTimer += Time.deltaTime;

//            bool swipeDetected = CheckSwipe(prevMousePos, Input.mousePosition);
//            swipeDir = (Input.mousePosition - prevMousePos).normalized;

//            if(swipeDetected.Equals(true))
//            {
//                Debug.Log(swipeDir + " " + prevMousePos);
//            }
//        }


//    }

//    private bool CheckSwipe(Vector3 downPos, Vector3 currentPos)
//    {
//        if (isSwiped.Equals(true))
//            return false;

//        Vector2 currentSwipe = currentPos - downPos;
//        if(currentSwipe.magnitude >= minSwipeDist)
//        {
//            Debug.Log(currentSwipe.magnitude);
//            isSwiped = true;
//            return true;
//        }

//        return false;
//    }


//}
