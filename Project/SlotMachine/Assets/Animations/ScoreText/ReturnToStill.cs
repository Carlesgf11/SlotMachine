using UnityEngine;

public class ReturnToStill : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void ReturnAnimToStill()
    {
        anim.SetTrigger("Return");
    }
}
