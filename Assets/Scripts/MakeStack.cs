using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStack : MonoBehaviour
{

    private Transform tr;
    private Vector3 pos;
    private Vector3 gameoverCam;

    Vector3 stackPosition;

    public GameObject stackPrefab;
    public GameObject stackPrefab_drop;
    public StackMover stackMover;
    public GameManager gameManager;

    public GameObject Base;

    private float randomColor;

    // Use this for initialization
    void Start()
    {
        tr = GetComponent<Transform>();
        pos = tr.position;
        //처음 색상 랜덤 배정 후, 베이스 색상 칠하기
        randomColor = Random.Range(0.0f, 1.0f);
        Base.GetComponent<MeshRenderer>().material.color = UnityEngine.Color.HSVToRGB(randomColor % 1.0f, 0.6f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() // 카메라 이동
    {
        if (!GameManager.checkGameOver)
        {
            CheckGameOver();

            if (Input.GetButtonDown("Fire1")) // 마우스 클릭 or 탭 할시
            {
                Makestack(); // 프리팹으로 스택 생성
                gameManager.GameScoreAdd(); // 스코어 증가

                if (gameManager.GetGameScore() > 0)
                    pos.y += 0.1f; // 카메라 y좌표 이동용 벡터            

                //Vector3 pos = tr.position; //transform.position; 
                // pos.y += 0.1f;
                // transform.position = pos; // 스택이 쌓이면서 변화가 필요한 카메라 위치 이동
            }
            tr.position = Vector3.Lerp(tr.position, pos, Time.deltaTime * 15.0f);
        }
        else
        {
            //Debug.Log(gameoverCam.x + " " + gameoverCam.z);
            tr.position = Vector3.Lerp(tr.position, gameoverCam, Time.deltaTime * 2.0f);
        }
    }


    Vector3 GetInstantiatePosition(string drop) // 왼쪽, 오른쪽 번갈아가면서 생성되기 위한 좌표 계산
    {
        if (drop == "stack")
        {
            float y = stackMover.GetPosition().y + (gameManager.GetGameScore() + 1) * 0.1f;
            if (gameManager.GetxzWhere())
            {
                float x = 1.5f;
                return new Vector3(x, y, GameManager.lastZ);
            }
            else
            {
                float z = 1.5f;
                return new Vector3(GameManager.lastX, y, z);
            }
        }
        else
        {
            // StackMover.cs에서 계산한 좌표를 가져온다
            // 계산된 좌표는 GameManager가 항상 가지고 있다
            float y = stackMover.GetPosition().y + (gameManager.GetGameScore()) * 0.1f;
            float x = GameManager.dropX;
            float z = GameManager.dropZ;

            return new Vector3(x, y, z);

        }
    }

    void Makestack() // 실제 프리팹 생성
    {
        // 프리팹이 2개 생성된다
        // 하나는 좌우로 움직이는 프리팹
        // 또 하나는 자르고 나서 떨어지는 하나의 프리팹

        GameObject nextStack = (GameObject)Instantiate(
            stackPrefab,
            GetInstantiatePosition("stack"),
            Quaternion.identity
            );

        nextStack.transform.parent = Base.transform;
        //생성된 스택 크기를 줄여줍니다.
        nextStack.transform.localScale = new Vector3(GameManager.lastXSize, 0.1f, GameManager.lastZSize);
        //색상 변환
        nextStack.GetComponent<MeshRenderer>().material.color = UnityEngine.Color.HSVToRGB(randomColor % 1.0f, 0.6f, 0.6f);


        if (gameManager.GetGameScore() >= 0)
        {
            Vector3 dropPosition = GetInstantiatePosition("drop");

            if (Mathf.Abs(dropPosition.x) != 0.0f)
            {
                stackPrefab_drop.transform.localScale = new Vector3(GameManager.dropXSize, 0.1f, GameManager.dropZSize);

                GameObject dropStack = (GameObject)Instantiate(
                    stackPrefab_drop,
                    dropPosition,
                    Quaternion.identity
                    );
                dropStack.GetComponent<MeshRenderer>().material.color = UnityEngine.Color.HSVToRGB(randomColor % 1.0f, 0.6f, 0.6f);
                randomColor += 0.02f;
            }

        }

    }

    public void CheckGameOver()
    {
        if (GameManager.checkGameOver)
        {
            //GameManager에서 게임 재시작
            GameManager.restartGame = true;
            //메인 카메라의 Local Z축 방향으로 움직이게 했습니다.
            //tr.position -= tr.forward * gameManager.GetGameScore() / 10.0f;


            //this.enabled = false;

        }
        else
        {
            // 게임 오버시 카메라 좌표 고정
            // 게임이 오버되면 이 좌표는 더 이상 변경되지 않는다.
            gameoverCam = tr.position;
            gameoverCam -= (tr.forward * gameManager.GetGameScore() / 10.0f);

        }

    }
}
