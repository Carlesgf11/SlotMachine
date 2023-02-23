using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotControl1 : MonoBehaviour
{
    //inst pos: y = 910 / destroy pos: y = -910
    [SerializeField] GameObject[] slotFigures;
    private int currentFigure;
    [SerializeField] GameObject slotDeTres;
    [SerializeField] Transform slotsParent;
    public bool isRunning;
    public Transform instPos, destroyPos, midPos;
    

    void Start()
    {
        isRunning = false;
        slotsParent.transform.GetChild(0).GetComponent<SlotDeTres>().amILast = true;
        currentFigure = slotFigures.Length - 1;
    }

    public void InstantiateNewSlotDeTres() //Instanciar nuevo slot de tres en la posición instPos y igualar variables necesarias
    {
        GameObject newThreeSlot = Instantiate(slotDeTres, instPos.position, Quaternion.identity);
        newThreeSlot.transform.SetParent(slotsParent.transform);
        newThreeSlot.transform.localScale = new Vector3(1, 1, 1);
        SetUpFigures(newThreeSlot.transform);
        newThreeSlot.GetComponent<SlotDeTres>().destroyPos = destroyPos;
        newThreeSlot.GetComponent<SlotDeTres>().midPos = midPos;
        newThreeSlot.GetComponent<SlotDeTres>().instPos = instPos;
        newThreeSlot.GetComponent<SlotDeTres>().slotControl1 = this;
    }

    public void SetUpFigures(Transform _slot) //Setear los slot de tres
    {
        CleanSlotDeTres(_slot);
        for (int figure = 3 - 1; figure >= 0; figure--)
        {
            if (currentFigure < 0)
                currentFigure = slotFigures.Length - 1;
            InstantiateFigure(_slot);
            currentFigure--;
        }
    }

    public void InstantiateFigure(Transform _slot) //Setear las figuras de cada slot de tres
    {
        GameObject newFigure = Instantiate(slotFigures[currentFigure], _slot.position, Quaternion.identity);
        newFigure.transform.SetParent(_slot);
        newFigure.transform.SetAsFirstSibling();
        newFigure.transform.localScale = new Vector3(1, 1, 1);
    }

    private void CleanSlotDeTres(Transform _slot) //Limpiar los nuevos slot de tres
    {
        for (int figura = _slot.transform.childCount - 1; figura >= 0; figura--)
        {
            Destroy(_slot.transform.GetChild(figura).gameObject);
        }
    }

    public void StopSpin()
    {
        if (isRunning)
        {
            slotsParent.transform.GetChild(1).GetComponent<SlotDeTres>().amILast = true;
            isRunning = false;
        }
    }

    public void StartSpin() 
    {
        if(!isRunning)
        {
            currentFigure = Random.Range(0, slotFigures.Length - 1);
            slotsParent.transform.GetChild(0).GetComponent<SlotDeTres>().amILast = false;
            InstantiateNewSlotDeTres();
            isRunning = true;
        }
    }
}
