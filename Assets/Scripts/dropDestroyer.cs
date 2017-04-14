using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropDestroyer : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DROPSTACK")
        {
            // 오브젝트 삭제
            Destroy(other.gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
