using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyDestroyer : MonoBehaviour {

    public CandyHolder candyHolder;
    //増やす個数
    public int reward;
    public GameObject effectPrefab;
    public Vector3 effectRotation;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Candy")
        {
            //指定数だけキャンディのストックを増やす
            candyHolder.AddCandy(reward);

            //オブジェクトを削除
            Destroy(other.gameObject);

            if (effectPrefab !=null)
            {
                //Candyのポジションにエフェクトを生成
                Instantiate(
                    effectPrefab,
                    other.transform.position,
                    Quaternion.Euler(effectRotation)
                    );

            }
        }
    }
}
