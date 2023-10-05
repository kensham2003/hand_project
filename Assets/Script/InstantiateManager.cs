using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class InstantiateManager : SingletonMonoBehaviour<InstantiateManager>
{
    public TextAsset cardMonsterCSV;
    private Dictionary<int, GameObject> cardMonsterDict = new Dictionary<int, GameObject>();

    //CPUメイン

    new void Awake(){
        base.Awake();
        //CSVファイルに<カードID、プレハブ名>のペアの想定
        ReadCardMonsterCSV();

        // //デバッグ用
        // foreach (KeyValuePair<int, GameObject> kvp in cardMonsterDict)
        //     Debug.Log ("カードID："+ kvp.Key + "  オブジェクト名：" + kvp.Value);
    }

    void ReadCardMonsterCSV(){
        //StringReaderを作成
        StringReader strReader = new StringReader(cardMonsterCSV.text);
        bool eof = false;
        //ファイルの最後まで読み込む
        while(!eof){
            string data_str = strReader.ReadLine();
            if(data_str == null){
                eof = true;
                break;
            }
            //一行の要素を配列に分割
            var values = data_str.Split(',');
            //読み込んだペアをDictionaryに追加
            cardMonsterDict.Add(Int32.Parse(values[0]), Resources.Load<GameObject>(values[1].ToString()));
        }
    }

    public void InstantiateMonster(int cardId, Vector3 position, Quaternion rotation){
        GameObject monster = cardMonsterDict[cardId];
        PoolManager.Instance.GetGameObject(monster, position, rotation);
    }

    public void InstantiateEffect(int effectId, Vector3 position, Quaternion rotation){
        //エフェクトの生成
    }
}
