using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    [SerializeField] GameObject[] slots;
    private float contadorTiempoSpin;
    private bool isSpining;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] Button spinBtn;
    [SerializeField] AudioManager audioManager;

    IEnumerator StartSpinMachine() //Empezar el spin
    {
        for (int slot = 0; slot<slots.Length; slot++)
        {
            slots[slot].GetComponent<SlotControl1>().StartSpin();
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator StopSpinMachine() //Parar el spin
    {
        for (int slot = 0; slot < slots.Length; slot++)
        {
            slots[slot].GetComponent<SlotControl1>().StopSpin();
            yield return new WaitForSeconds(0.3f);
        }
        audioManager.StopSound("Tragaperras");
        audioManager.PlaySound("StopSlot");
        yield return new WaitForSeconds(0.2f);
        scoreManager.ChangeScoreGrid();
    }

    public void Spin() { isSpining = true; audioManager.PlaySound("Tragaperras"); spinBtn.interactable = false; } // Función que tiene asignada el botón spin

    private void SpinUpdate()
    {
        StartCoroutine("StartSpinMachine");
        contadorTiempoSpin += Time.deltaTime;
        float randomSec = Random.Range(2f, 5f);
        if (contadorTiempoSpin >= randomSec)
        {
            StartCoroutine("StopSpinMachine");
            contadorTiempoSpin = 0;
            isSpining = false;
        }
    }

    private void Update()
    {
        if (isSpining)
            SpinUpdate();
    }
}
