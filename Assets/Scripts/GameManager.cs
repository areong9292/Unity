using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // 스코어 등과 같은 게임 외적인 것들 제어
    // 및 게임에 필요한 내용을 가지고 있다.

    public static int GameScore;
    public static bool xzWhere;  // true시 x축 왔다 갔다
                                 // false시 z축 왔다 갔다
    public static float lastX;
    public static float lastZ;

    public static float lastXSize;
    public static float lastZSize;

    public static float dropX;
    public static float dropZ;

    public static float dropXSize;
    public static float dropZSize;

    // 여지껏 쌓은 스택을 한번 보여주는 단계를 거치고 재시작하다보니
    // 2개의 bool 변수가 필요했고, 이름을 변경해야될 필요를 느꼈습니당
    public static bool restartGame;
    public static bool checkGameOver;

    // Use this for initialization
    void Start()
    {
        GameScore = -1;

        lastX = 0.0f;
        lastZ = 0.0f;

        lastXSize = 1.0f;
        lastZSize = 1.0f;

        dropX = 0.0f;
        dropZ = 0.0f;

        dropXSize = 1.0f;
        dropZSize = 1.0f;

        restartGame = false;
        checkGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // 마우스 클릭 or 탭 할시
        {
            CheckRestartGame(); // 게임이 종료되서 멈춰있는 상태면 재시작해줍니다.
            xzWhere = !xzWhere; // bool flag를 이용한 좌우 변환
        }
    }

    public int GetGameScore()
    {
        return GameScore;
    }

    public void GameScoreAdd()
    {
        GameScore++;
    }

    public bool GetxzWhere()
    {
        return xzWhere;
    }

    public void CheckRestartGame()
    {
        if(restartGame==true)
        {
            SceneManager.LoadScene("Stack");
        }
    }
}
