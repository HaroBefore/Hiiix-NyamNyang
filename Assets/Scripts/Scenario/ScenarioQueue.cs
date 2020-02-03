using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioQueue {
    Queue<Scenario> bossQ;
    Queue<Scenario> eventQ;
    Queue<Scenario> nyangQ;

    public ScenarioQueue() {
        bossQ = new Queue<Scenario>();
        eventQ = new Queue<Scenario>();
        nyangQ = new Queue<Scenario>();
    }

    public void Enqueue(Scenario s) {
        switch (s.type) {
            case ScenarioType.Boss:
                bossQ.Enqueue(s);
                break;
            case ScenarioType.Event:
                eventQ.Enqueue(s);
                break;
            case ScenarioType.Nyang://
                nyangQ.Enqueue(s);
                break;
        }
    }

    public Scenario Dequeue() {
        if (nyangQ.Count != 0) return nyangQ.Dequeue();
        if (eventQ.Count != 0) return eventQ.Dequeue();
        if (bossQ.Count != 0) return bossQ.Dequeue();
        return null;
    }

    public int GetCount() {
        return bossQ.Count + eventQ.Count + nyangQ.Count;
    }

    public bool IsEmpty() {
        return GetCount() == 0;
    }


}