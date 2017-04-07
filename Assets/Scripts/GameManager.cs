using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // 스코어 등과 같은 게임 외적인 것들 제어
    // 및 게임에 필요한 내용을 가지고 있다.

    public static int GameScore = -1;
    public bool xzWhere;  // true시 x축 왔다 갔다
                          // false시 z축 왔다 갔다
    public static float last_x = 0.0f;
    public static float last_z = 0.0f;
    
    public static float last_x_scale = 1.0f;
    public static float last_z_scale = 1.0f;

    public static float drop_x = 0.0f;
    public static float drop_z = 0.0f;

    public static float drop_x_scale = 1.0f;
    public static float drop_z_scale = 1.0f;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // 마우스 클릭 or 탭 할시
        {
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
}
