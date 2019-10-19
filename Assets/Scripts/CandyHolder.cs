using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyHolder : MonoBehaviour {

    //初期所持数
    const int DefaultCandyAmount = 30;

    //時間経過で回復する
    const int RecoverySeconds = 10;

    //現在のキャンディ数
    int candy = DefaultCandyAmount;

    //ストック回復までの残り秒数
    int counter;

    public void ConsumeCandy()
    {
        //キャンディが０よりおおきければ１減らす
        if (candy > 0) candy--;
    }

    public int GetCandyAmount()
    {
        //candyの値を返す
        return candy;
    }

    public void AddCandy(int amount)
    {
        candy += amount;
    }

    private void OnGUI()
    {
        GUI.color = Color.black;

        //キャンディのストック数
        string label = "Candy :" + candy;

        //回復している時だけ秒数を表示
        if (counter > 0) label = label + "(" + counter + "s)";

        GUI.Label(new Rect(0, 0, 100, 30), label);
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //キャンディのストックがデフォルトより少なく
        //回復カウントしていない時にカウントをスタートさせる
        if (candy < DefaultCandyAmount && counter <= 0)
        {
            //コルーチンのスタート
            StartCoroutine(RecoverCandy());
        }
	}

    IEnumerator RecoverCandy()
    {
        counter = RecoverySeconds;

        //１秒づつカウントを進める
        while (counter >0)
        {
            yield return new WaitForSeconds(1.0f);
            counter--;

        }

        candy++;
    }
}
