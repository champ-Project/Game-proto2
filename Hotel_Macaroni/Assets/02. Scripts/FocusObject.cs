using UnityEngine;

public class FocusObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform targetTransform;
    public void Interact()
    {
        if(targetTransform != null)
        {
            PlayerController playerController = GameManager.instance.playerController;
            if(playerController != null)
            {
                playerController.FocusTarget(targetTransform, this.gameObject);
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
