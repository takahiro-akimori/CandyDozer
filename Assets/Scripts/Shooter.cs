using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {

    const int SphereCandyFrequency = 3;
    //連続発射可能個数
    const int MaxShotPower = 5;
    //回復にかかる時間
    const int RecoverySeconds = 3;

    int sampleCandyCount;
    int shotPower = MaxShotPower;
    AudioSource shotSound;

    //CandyPrefab
    public GameObject[] candyPrefabs;
    public GameObject[] candySquarePrefabs;
    public CandyHolder candyHolder;
    public float shotSpeed;
    public float shotTorque;
    public float baseWidth;

	// Use this for initialization
	void Start () {
        //AudioSourceからサウンドの情報を取得
        shotSound = GetComponent<AudioSource>();
		
	}
	
	// Update is called once per frame
	void Update () {

        //入力があった時にshot関数を呼ぶ
        if (Input.GetButtonDown("Fire1")) Shot();
	}

    //キャンディのPrefabをランダムにえらぶ
    GameObject SampleCandy　()
    {
        GameObject prefab = null;

        //特定回数に一回丸いキャンディを選択する
        if (sampleCandyCount % SphereCandyFrequency == 0)
        {
            //Random.Rangeで０〜candyPrefabsの要素数からランダムな整数を代入
            //candyPrefabが５種類なら０〜４の整数を代入
            //Lengthは要素数を取得
            int index = Random.Range(0, candyPrefabs.Length);
            //ランダムに決めたindexを配列の中から選びgameobjectのprefabに代入
            prefab = candyPrefabs[index];
        }else {
            int index = Random.Range(0, candySquarePrefabs.Length);
            prefab = candySquarePrefabs[index];
        }

        sampleCandyCount++;

        return prefab;
    }

    Vector3 GetInstantiatePosition()
    {
        //画面のサイズとInputの割合からキャンディ生成の位置を計算
        float x = baseWidth *
            (Input.mousePosition.x / Screen.width) - (baseWidth / 2);
        return transform.position + new Vector3(x, 0, 0);
    }

    public void Shot() {

        //キャンディを生成できる条件外の時はshotしない
        if (candyHolder.GetCandyAmount() <= 0) return;
        if (shotPower <= 0) return;

        //PrefabからCandyオブジェクトを生成
        GameObject candy = (GameObject)Instantiate(
            SampleCandy(),              //何を生成
            GetInstantiatePosition(),   //どこに生成
            Quaternion.identity         //回転なし
            );

        //生成したCandyオブジェクトの親をCandyHolderに設定する
        candy.transform.parent = candyHolder.transform;

        //CandyオブジェクトのRigidbodyを取得、力と回転を加える
        Rigidbody candyRigidBody = candy.GetComponent<Rigidbody>();
        candyRigidBody.AddForce(transform.forward * shotSpeed);
        candyRigidBody.AddTorque(new Vector3(0, shotTorque, 0));

        //Candyのストックを消費
        candyHolder.ConsumeCandy();
        //shotPowerを消費
        ConsumePower();

        //サウンドを再生
        shotSound.Play();
    }

    //shotPowerの表示
    private void OnGUI()
    {
        GUI.color = Color.black;
        

        //ShotPowerの残数を＋で表示
        string label = "";
        for (int i = 0; i < shotPower; i++) label = label + "+";

        GUI.Label(new Rect(0, 15, 100, 30), label);
    }

    //shotPowerの消費処理
    void ConsumePower()
    {
        //shotPowerを消費すると同時にカウントをスタート
        shotPower --;
        StartCoroutine(RecoverPower());
    }

    //shotPowerの回復コルーチン
    IEnumerator RecoverPower()
    {
        //一定秒数待った後にshotPowerを回復
        yield return new WaitForSeconds(RecoverySeconds);
        shotPower++;
    }
}
