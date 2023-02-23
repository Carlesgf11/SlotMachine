using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDeTres : MonoBehaviour
{
    public Transform instPos, destroyPos, midPos;
    private float speed = 3600f; //Velocidad Unity: 2900 / Velocidad build: 3600
    public SlotControl1 slotControl1;
    public bool amILast;
    public bool instantiateLastOne;

    void Update()
    {
        if (!amILast)
            RunningUpdate();
        else
            transform.position = Vector3.MoveTowards(transform.position, midPos.position, speed * Time.deltaTime);
    }

    private void RunningUpdate()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < destroyPos.position.y)
        {
            if(slotControl1.isRunning)
                slotControl1.InstantiateNewSlotDeTres();
            Destroy(gameObject);
        }
    }
}
