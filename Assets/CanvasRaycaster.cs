using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CanvasRaycaster : MonoBehaviour
{

    private Card nowHovering = null;

    void Update()
    {
        //カード選択中
        if(nowHovering != null && Input.GetMouseButton(0)){
            //選択中カードを一番上に表示
            nowHovering.transform.SetAsLastSibling(); 
            return;
        }

        // デバッグ用
        // if(nowHovering == null){
        //     Debug.Log("null");
        // }
        // else{
        //     Debug.Log(nowHovering.gameObject.name);
        // }

        bool isHoveringCard = false;

        //RaycastAllの引数（PointerEventData）作成
        PointerEventData pointData = new PointerEventData(EventSystem.current);

        //RaycastAllの結果格納用List
        List<RaycastResult> RayResult= new List<RaycastResult>(); 

        //PointerEventDataにマウスの位置をセット
        pointData.position = Input.mousePosition;
        //RayCast（スクリーン座標）
        EventSystem.current.RaycastAll(pointData , RayResult);

        //当たっているカードだけホバーするように
        foreach (RaycastResult result in RayResult)
        {
            Card card = result.gameObject.GetComponent<Card>();
            if(!card)continue;

            //すでにホバーしているならこのままに
            if(nowHovering == card){
                card.horverd = true;
                isHoveringCard = true;
                break;
            }

            //ホバーしていないならホバーにする
            if(!nowHovering){
                card.horverd = true;
                nowHovering = card;
                //選択中カードを一番上に表示
                card.transform.SetAsLastSibling(); 
                isHoveringCard = true;
            }
        }

        //他のカードをホバーしていない状態に
        foreach(Card card in GetComponentsInChildren<Card>()){
            if(card == nowHovering)continue;
            card.horverd = false;
        }

        //なにもホバーしていないならnullに
        if(!isHoveringCard){
            nowHovering = null;
        }
    }
}
