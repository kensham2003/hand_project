using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class InstantiateManager : MonoBehaviour
{
    public TextAsset cardMonsterCSV;
    private Dictionary<int, GameObject> cardMonsterDict = new Dictionary<int, GameObject>();

    private Dictionary<int, MonsterParamerter> cardMonsterParamDict = new Dictionary<int, MonsterParamerter>();

    [SerializeField]private VisibleList visibleList;

    //CPUメイン
    [SerializeField]private CpuMain cpuMain;

    // new void Awake(){
    //     base.Awake();
    //     //CSVファイルに<カードID、プレハブ名>のペアの想定
    //     ReadCardMonsterCSV();

    //     visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
    //     // //デバッグ用
    //     // foreach (KeyValuePair<int, GameObject> kvp in cardMonsterDict)
    //     //     Debug.Log ("カードID："+ kvp.Key + "  オブジェクト名：" + kvp.Value);
    // }
    private void Awake(){
        //CSVファイルに<カードID、プレハブ名>のペアの想定
        ReadCardMonsterCSV();

        visibleList = GameObject.Find("Managers").GetComponent<VisibleList>();
        // //デバッグ用
        // foreach (KeyValuePair<int, GameObject> kvp in cardMonsterDict)
        //     Debug.Log ("カードID："+ kvp.Key + "  オブジェクト名：" + kvp.Value);
    }

    void ReadCardMonsterCSV(){
        //StringReaderを作成
        StringReader strReader = new StringReader(cardMonsterCSV.text);
        bool eof = false;
        //項目名の行を省略
        strReader.ReadLine();
        //ファイルの最後まで読み込む
        while(!eof){
            string data_str = strReader.ReadLine();
            if(data_str == null){
                eof = true;
                break;
            }
            //一行の要素を配列に分割
            var values = data_str.Split(',');
            //読み込んだペアをDictionaryに追加                    //CSVファイル各列の項目
            cardMonsterDict.Add(Int32.Parse(values[0]),         //1. ID
             Resources.Load<GameObject>(values[1].ToString())   //2. ファイル名
             );
            MonsterParamerter mp = new MonsterParamerter{
                hp = Int32.Parse(values[2]),                    //3. HP
                speed = float.Parse(values[3]),                 //4. スピード
                attack = Int32.Parse(values[4]),                //5. 攻撃力
                attackDistance = float.Parse(values[5]),        //6. 攻撃距離
                attackInterval = float.Parse(values[6]),        //7. 攻撃CT
                constantLoad = new CPULoad{
                    raiseRate = float.Parse(values[7]),         //8. 常時上昇率
                    impactTime = -1
                    },
                spawnLoad = new CPULoad{
                    raiseRate = float.Parse(values[8]),         //9. 出現上昇（率）
                    impactTime = float.Parse(values[9])         //10.出現上昇（時間）
                    },
                attackLoad = new CPULoad{
                    raiseRate = float.Parse(values[10]),        //11.攻撃上昇（率）
                     impactTime = float.Parse(values[11])       //12.攻撃上昇（時間）
                     },
                DestroyLoad = new CPULoad{
                    raiseRate = float.Parse(values[12]),        //13.消失上昇（率）
                    impactTime = float.Parse(values[13])        //14.消失上昇（時間）
                    }
            };
            cardMonsterParamDict.Add(Int32.Parse(values[0]), mp);
        }
    }

    public void InstantiateMonster(int cardId, Vector3 position, Quaternion rotation){
        GameObject monster = cardMonsterDict[cardId];
        GameObject monsterObj = PoolManager.Instance.GetGameObject(monster, position, rotation);
        Monster m = monsterObj.GetComponent<Monster>();
        m.visibleList = visibleList;
        m.cpuMain = cpuMain;
        m.instantiateManager = this;
        m.paramerter = cardMonsterParamDict[cardId];
        cpuMain.UsageRegister(m.paramerter.spawnLoad);
        Debug.Log("生成 : " + m.paramerter.spawnLoad.raiseRate);
    }

    public void DestroyMonster(GameObject monster){
        PoolManager.Instance.ReleaseGameObject(monster);
        cpuMain.UsageRegister(monster.GetComponent<Monster>().paramerter.DestroyLoad);
        Debug.Log("消失 : " + monster.GetComponent<Monster>().paramerter.DestroyLoad.raiseRate);
    }

    public void InstantiateEffect(int effectId, Vector3 position, Quaternion rotation){
        //エフェクトの生成
    }
}
