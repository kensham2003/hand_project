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
        //CSVファイルに<カードID、ファイル名>のペアの想定

        //ここは試しとして直接Dictに入れる
        cardMonsterDict.Add(1, Resources.Load<GameObject>("TestObject"));
    }

    void ReadCardMonsterCSV(){
        StreamReader strReader = new StreamReader(cardMonsterCSV.text);
        bool eof = false;
        while(!eof){
            string data_str = strReader.ReadLine();
            if(data_str == null){
                eof = true;
                break;
            }
            var values = data_str.Split(',');

            //未テスト
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
