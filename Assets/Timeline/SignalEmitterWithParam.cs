using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class SignalEmitterWithBool : ParameterizedEmitter<bool>{}
public class SignalEmitterWithInt : ParameterizedEmitter<int>{}

public class SignalEmitterWithTwoInt : ParameterizedEmitterWithTwoParams<int>{}

public class ParameterizedEmitter<T> : SignalEmitter{
    public T parameter;
}

public class ParameterizedEmitterWithTwoParams<T> : SignalEmitter{
    public T parameter1;
    public T parameter2;
}