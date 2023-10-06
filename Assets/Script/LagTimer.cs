using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LagTimer
{
    static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100);
 
    static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame OneFrame {
        get{ return _endOfFrame;}
    }
 
    static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    public static WaitForFixedUpdate FixedUpdate{
        get{ return _fixedUpdate; }
    }
 
    public static WaitForSeconds Get(float seconds){
        if(!_timeInterval.ContainsKey(seconds))
            _timeInterval.Add(seconds, new WaitForSeconds(seconds));
        return _timeInterval[seconds];
    }
}
