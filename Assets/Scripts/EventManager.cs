using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public delegate void JoinBandAction(string friendType);
    public static event JoinBandAction OnJoinBand;

    public delegate void LeaveBandAction(string friendType);
    public static event LeaveBandAction OnLeaveBand;

    public void JoinBand(string friendType)
    {
        if (EventManager.OnJoinBand != null)
        {
            EventManager.OnJoinBand(friendType);
        }
    }

    public void LeaveBand(string friendType)
    {
        if (EventManager.OnLeaveBand != null)
        {
            EventManager.OnLeaveBand(friendType);
        }
    }
}
