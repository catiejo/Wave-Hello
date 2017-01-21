using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    public delegate void JoinBandAction(Friend friend);
    public static event JoinBandAction OnJoinBand;

    public delegate void LeaveBandAction(Friend friend);
    public static event LeaveBandAction OnLeaveBand;

    public void JoinBand(Friend friend)
    {
        if (EventManager.OnJoinBand != null)
        {
            EventManager.OnJoinBand(friend);
        }
    }

    public void LeaveBand(Friend friend)
    {
        if (EventManager.OnLeaveBand != null)
        {
            EventManager.OnLeaveBand(friend);
        }
    }
}
