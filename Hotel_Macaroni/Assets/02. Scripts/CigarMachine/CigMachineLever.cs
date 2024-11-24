using UnityEngine;

public class CigMachineLever : MonoBehaviour, IInteractable
{
    public CigMachine cigarMachine;
    public int leverNum;

    public void Interact()
    {
        if(cigarMachine != null)
        {
            cigarMachine.LeverAction(leverNum);
        }
    }
}
