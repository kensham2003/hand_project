using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//メモリーのゴミを減らすためWaitForSecondsRealTimeのキャッシュ
public static class LagTimer
{
    static Dictionary<float, WaitForSecondsRealtime> _timeInterval = new Dictionary<float, WaitForSecondsRealtime>(100);
 
    static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame OneFrame {
        get{ return _endOfFrame;}
    }
 
    static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    public static WaitForFixedUpdate FixedUpdate{
        get{ return _fixedUpdate; }
    }
 
    //待つ時間を取得
    public static WaitForSecondsRealtime Get(float seconds){
        if(!_timeInterval.ContainsKey(seconds))
            _timeInterval.Add(seconds, new WaitForSecondsRealtime(seconds));
        return _timeInterval[seconds];
    }
}