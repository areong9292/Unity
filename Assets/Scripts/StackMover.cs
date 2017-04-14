using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackMover : MonoBehaviour
{

    private Transform tr;
    private Rigidbody rigid;

    Vector3 startPosition;
    Vector3 dropScale;

    public float amplitude;
    public float speed;
    public float x, z;
    public bool isCollision;

    public bool checkGameOver;
    public bool xzWhere;
    public float lastX, lastZ, lastXSize, lastZSize;
    public float dropX, dropZ, dropXSize, dropZSize;

    float testTime;

    // Use this for initialization
    void Start()
    {
        tr = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();

        testTime = Time.time;
        startPosition = tr.localPosition; // 초기위치 보관

        xzWhere = GameManager.xzWhere;
        lastX = GameManager.lastX;
        lastZ = GameManager.lastZ;
        lastXSize = GameManager.lastXSize;
        lastZSize = GameManager.lastZSize;
        dropX = GameManager.dropX;
        dropZ = GameManager.dropZ;
        dropXSize = GameManager.dropXSize;
        dropZSize = GameManager.dropZSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // 마우스 클릭 or 탭 할시
        {
            if (isCollision) // 충돌 상태이면
            {

                if (xzWhere) //x축 움직임
                {
                    if (Mathf.Abs(x-lastX)<0.05)
                    {
                        tr.localPosition = new Vector3(lastX,
                             startPosition.y, lastZ);

                        GameManager.dropZSize = 0.0f;
                        GameManager.dropXSize = 0.0f;

                        GameManager.dropX = 0.0f;
                        GameManager.dropZ = 0.0f;
                        
                    }
                    else
                    {
                        // 잘려지는 부분과 남는 부분의 scale 계산
                        tr.localScale = new Vector3(lastXSize - Mathf.Abs(x - lastX),
                             0.1f, lastZSize);
                        dropScale = new Vector3(Mathf.Abs(x - lastX), 0.1f, lastZSize);

                        // 잘려지는 부분과 남는 부분의 position 계산
                        tr.localPosition = new Vector3(lastX + ((x - lastX) / 2.0f),
                             startPosition.y, lastZ);

                        // position의 경우 왼쪽, 오른쪽으로 잘려질 때 좌표값이 확 바뀌기 때문에 나눔
                        float check_x;
                        if (x > lastX) // 카메라 기준으로 왼쪽에서 잘릴때
                        {
                            check_x = (x + lastX + lastXSize) / 2.0f;
                        }

                        else
                        {
                            check_x = (x + lastX - lastXSize) / 2.0f;
                        }

                        GameManager.lastX = tr.localPosition.x;
                        GameManager.lastXSize = tr.localScale.x;

                        GameManager.dropZSize = tr.localScale.z;
                        GameManager.dropXSize = dropScale.x;

                        GameManager.dropX = check_x;
                        GameManager.dropZ = tr.localPosition.z;
                        //Debug.Log("debug : "+ GameManager.last_x+" " + GameManager.last_x_scale);
                    }

                }
                else // z축 움직임
                {
                    if (Mathf.Abs(z - lastZ) < 0.05)
                    {
                        tr.localPosition = new Vector3(lastX,
                             startPosition.y, lastZ);

                        GameManager.dropZSize = 0.0f;
                        GameManager.dropXSize = 0.0f;

                        GameManager.dropX = 0.0f;
                        GameManager.dropZ = 0.0f;
                        
                    }
                    else
                    {

                        tr.localScale = new Vector3(lastXSize, 0.1f,
                            lastZSize - Mathf.Abs(z - lastZ));

                        dropScale = new Vector3(lastXSize, 0.1f, Mathf.Abs(z - lastZ));

                        tr.localPosition = new Vector3(lastX, startPosition.y,
                            lastZ + ((z - lastZ) / 2.0f));

                        float check_z;
                        if (z > lastZ) // 카메라 기준으로 왼쪽에서 잘릴때
                        {
                            check_z = (z + lastZ + lastZSize) / 2.0f;
                        }
                        else
                        {
                            check_z = (z + lastZ - lastZSize) / 2.0f;
                        }

                        GameManager.lastZ = tr.localPosition.z;
                        GameManager.lastZSize = tr.localScale.z;

                        GameManager.dropXSize = tr.localScale.x;
                        GameManager.dropZSize = dropScale.z;

                        GameManager.dropZ = check_z;
                        GameManager.dropX = tr.localPosition.x;
                        //Debug.Log("debug : " + GameManager.last_z + " " + GameManager.last_z_scale);
                    }
                }
            }

            else // Game Over
            {
                GameManager.checkGameOver = true;
                /*기본 Stack Prefab의 설정이 충돌시 흔들림 방지를 위해 
                 * rigidbody에 constraints를 다 걸어버리는 식으로 구현했습니다.
                 * 그러다보니 GameOver를 유발한 마지막 Stack이 공중에 뜬 채로 종료되서
                 * 마지막 스택만 중력 적용해주고 constraints를 풀어주었습니다. */ 
                rigid.useGravity = true;
                rigid.constraints = RigidbodyConstraints.None;
            }

            this.enabled = false; // 이전 블록의 스크립트 비활성화
        }
        else if (xzWhere)
        {
            x = amplitude * Mathf.Sin((Time.time - testTime + 1f) * speed); // 이동량 계산
            tr.localPosition = new Vector3(x, startPosition.y, lastZ); // 포지션 반영

            //tr.position = Vector3.Lerp(tr.position,new Vector3(x,startPosition.y,0),Time.deltaTime);
        }
        else
        {
            z = amplitude * Mathf.Sin((Time.time - testTime + 1f) * speed); // 이동량 계산
            tr.localPosition = new Vector3(lastX, startPosition.y, z); // 포지션 반영
        }
    }

    void OnCollisionEnter(Collision stack)
    {
        if (stack.collider.tag == "STACK")
        {
            isCollision = true;
        }
    }

    void OnCollisionExit(Collision stack)
    {
        if (stack.collider.tag == "STACK")
        {
            isCollision = false;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.localPosition;
    }
}
