using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStack : MonoBehaviour {

    private Transform tr;
    private Vector3 pos;


    Vector3 stackPosition;

    public GameObject stackPrefab;
    public GameObject stackPrefab_drop;
    public StackMover stackMover;
    public GameManager gameManager;

    public GameObject Base;

    // Use this for initialization
    void Start () {
        tr = GetComponent<Transform>();
        pos = tr.position;
    }
	
	// Update is called once per frame
	void Update()
    {
        
    }

    void LateUpdate() // 카메라 이동
    {
        if (Input.GetButtonDown("Fire1")) // 마우스 클릭 or 탭 할시
        {
            Makestack(); // 프리팹으로 스택 생성
            gameManager.GameScoreAdd(); // 스코어 증가

           
            pos.y += 0.1f; // 카메라 y좌표 이동용 벡터
            
            

            //Vector3 pos = tr.position; //transform.position; 
            // pos.y += 0.1f;
            // transform.position = pos; // 스택이 쌓이면서 변화가 필요한 카메라 위치 이동
        }
        tr.position = Vector3.Lerp(tr.position, pos, Time.deltaTime * 15.0f);
    }



    Vector3 GetInstantiatePosition(string drop) // 왼쪽, 오른쪽 번갈아가면서 생성되기 위한 좌표 계산
    {
        if (drop == "stack")
        {
            float y = stackMover.GetPosition().y + (gameManager.GetGameScore()+1) * 0.1f;
            if (gameManager.GetxzWhere())
            {
                float x = 1.5f;
                return new Vector3(x, y, GameManager.last_z);
            }
            else
            {
                float z = 1.5f;
                return new Vector3(GameManager.last_x, y, z);
            }
        }
        else
        {
            // StackMover.cs에서 계산한 좌표를 가져온다
            // 계산된 좌표는 GameManager가 항상 가지고 있다
            float y = stackMover.GetPosition().y + (gameManager.GetGameScore()) * 0.1f;   
            float x = GameManager.drop_x;
            float z = GameManager.drop_z;

            return new Vector3(x, y, z);
           
        }
    }

    void Makestack() // 실제 프리팹 생성
    {
        // 프리팹이 2개 생성된다
        // 하나는 좌우로 움직이는 프리팹
        // 또 하나는 자르고 나서 떨어지는 하나의 프리팹

        GameObject stack = (GameObject)Instantiate(
            stackPrefab,
            GetInstantiatePosition("stack"),
            Quaternion.identity
            );

        stack.transform.parent = Base.transform;
        //생성된 스택 크기를 줄여줍니다.
        stack.transform.localScale = new Vector3(GameManager.last_x_scale, 0.1f, GameManager.last_z_scale);

        
        if (gameManager.GetGameScore() >= 0)
        {
            stackPrefab_drop.transform.localScale = new Vector3(GameManager.drop_x_scale, 0.1f, GameManager.drop_z_scale);
            
            GameObject stack1 = (GameObject)Instantiate(
                stackPrefab_drop,
                GetInstantiatePosition("drop"),
                Quaternion.identity
                );
        }
            

    }
}
