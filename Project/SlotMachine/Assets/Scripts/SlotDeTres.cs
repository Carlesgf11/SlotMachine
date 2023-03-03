using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDeTres : MonoBehaviour
{
    public Transform instPos, destroyPos, midPos;
    private float speed = 550f; //Velocidad Unity: 2900 / Velocidad build: 3600 / Velocidad WebGL: 550
    public SlotControl1 slotControl1;
    public bool amILast;

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
