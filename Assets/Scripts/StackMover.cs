using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMover : MonoBehaviour {

    private Transform tr;
    private Renderer rend;
    
    Vector3 startPosition;

    Vector3 dropScale;
    Vector3 dropPosition;


    public float amplitude;
    public float speed;
    public bool is_collision;
    public bool along_x_axis;
    public float x, z;

    float testTime;
    

    // Use this for initialization
    void Start()
    {
        tr = GetComponent<Transform>();
        rend = GetComponent<Renderer>();

        along_x_axis = GameObject.Find("GameManager").GetComponent<GameManager>().xzWhere;
        testTime = Time.time;
        startPosition = tr.localPosition; // 초기위치 보관
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // 마우스 클릭 or 탭 할시
        {
            if (is_collision) // 충돌 상태이면
            {
                
                if (along_x_axis) //x축 움직임
                {
                    // 잘려지는 부분과 남는 부분의 scale 계산
                    tr.localScale = new Vector3(GameManager.last_x_scale - Mathf.Abs(x - GameManager.last_x), 
                         0.1f, GameManager.last_z_scale);
                    dropScale = new Vector3(Mathf.Abs(x - GameManager.last_x), 0.1f, GameManager.last_z_scale);

                    // 잘려지는 부분과 남는 부분의 position 계산
                    tr.localPosition = new Vector3(GameManager.last_x + ((x - GameManager.last_x) / 2.0f),
                         startPosition.y, GameManager.last_z);

                    // position의 경우 왼쪽, 오른쪽으로 잘려질 때 좌표값이 확 바뀌기 때문에 나눔
                    float check_x;
                    if(x>0) // 카메라 기준으로 왼쪽에서 잘릴때
                    {
                        check_x = (x+GameManager.last_x+GameManager.last_x_scale)/2.0f;
                    }

                    else
                    {
                        check_x = (x + GameManager.last_x - GameManager.last_x_scale) / 2.0f;
                    }

                    GameManager.last_x = tr.localPosition.x;
                    GameManager.last_x_scale = tr.localScale.x;

                    GameManager.drop_z_scale = tr.localScale.z;
                    GameManager.drop_x_scale = dropScale.x;

                    GameManager.drop_x = check_x;
                    GameManager.drop_z = tr.localPosition.z;
                    //Debug.Log("debug : "+ GameManager.last_x+" " + GameManager.last_x_scale);
                }
                else // z축 움직임
                {
                    tr.localScale = new Vector3(GameManager.last_x_scale, 0.1f,
                        GameManager.last_z_scale - Mathf.Abs(z - GameManager.last_z));

                    dropScale = new Vector3(GameManager.last_x_scale, 0.1f, Mathf.Abs(z - GameManager.last_z));

                    tr.localPosition = new Vector3(GameManager.last_x, startPosition.y,
                        GameManager.last_z + ((z - GameManager.last_z) / 2.0f));

                    dropPosition= new Vector3(GameManager.last_x, startPosition.y,
                        GameManager.last_z + ((z - GameManager.last_z) / 2.0f));

                    float check_z;
                    if (z > 0) // 카메라 기준으로 왼쪽에서 잘릴때
                    {
                        check_z = (z + GameManager.last_z + GameManager.last_z_scale) / 2.0f;
                    }
                    else
                    {
                        check_z = (z + GameManager.last_z - GameManager.last_z_scale) / 2.0f;
                    }
                        

                    GameManager.last_z = tr.localPosition.z;
                    GameManager.last_z_scale = tr.localScale.z;


                    GameManager.drop_x_scale = tr.localScale.x;
                    GameManager.drop_z_scale = dropScale.z;

                    GameManager.drop_z = check_z;
                    GameManager.drop_x = tr.localPosition.x;
                    //Debug.Log("debug : " + GameManager.last_z + " " + GameManager.last_z_scale);
                }
            }
            else // Game Over
            {
                Debug.Log("Game Over");
            }

            this.enabled = false; // 이전 블록의 스크립트 비활성화
        }
        else if (along_x_axis)
        {
            x = amplitude * Mathf.Sin((Time.time - testTime + 1f) * speed); // 이동량 계산
            tr.localPosition = new Vector3(x, startPosition.y, GameManager.last_z); // 포지션 반영
            
            //tr.position = Vector3.Lerp(tr.position,new Vector3(x,startPosition.y,0),Time.deltaTime);
        }
        else
        {
            z = amplitude * Mathf.Sin((Time.time - testTime + 1f) * speed); // 이동량 계산
            tr.localPosition = new Vector3(GameManager.last_x, startPosition.y, z); // 포지션 반영
        }
    }

    void OnCollisionEnter(Collision stack)
    {
        if (stack.collider.tag == "STACK")
        {
            is_collision = true;
            rend.material.color = Color.black;
        }
    }

    void OnCollisionExit(Collision stack)
    {
        if (stack.collider.tag == "STACK")
        {
            is_collision = false;
            rend.material.color = Color.white;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.localPosition;
    }      

}
