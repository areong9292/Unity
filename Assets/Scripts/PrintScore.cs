using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintScore : MonoBehaviour {

    public GameManager gameManager;

    Text scoreLabel;
    // Use this for initialization
    void Start() {
        // text 내용 컴포넌트 받아옴
        scoreLabel = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        // 매프레임마다 갱신
        ChangeText();
    }

    void ChangeText()
    {
        // 처음에 프리팹생성문제로 스코어 값을 -1로 해둠
        if (gameManager.GetGameScore() != -1)
            scoreLabel.text = gameManager.GetGameScore().ToString();
        else
            scoreLabel.text = "0";
    }
}
