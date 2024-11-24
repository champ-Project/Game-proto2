using UnityEngine;
using UnityEngine.UI;

//�÷��̾� ������ ��ũ��Ʈ
//Ray, Ÿ�� ������Ʈ ����, ������Ʈ Ȯ�� ��
public class ReticleManager : MonoBehaviour
{
    public float rayDistance = 3f;          //Ray�Ÿ�
    private GameObject currentItem;         //���� �������� ���� ������
    private bool isReticalOnItem = false;   //������ ���� ���� bool
    public GameObject reticleText;          //������ �ؽ�Ʈ
    public GameObject pointerIcon;          //������ ������ ������Ʈ
    public Image iconImage;                 //������ �̹��� ������Ʈ
    public Sprite[] iconSprites;            //������ ��������Ʈ

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;

        if (reticleText.activeSelf == true)
        {
            reticleText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectItem();
    }

    //Ray�� ������ ����, currentItem�� �ӽ� ����
    private void DetectItem()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, rayDistance))
        {
            if (currentItem != hit.collider.gameObject)
            {
                currentItem = hit.collider.gameObject;
                isReticalOnItem = true;

                if (currentItem.CompareTag("Item"))
                {
                    ShowInteractUI("item", true);
                    Debug.Log("������ ����" + currentItem.name);
                }
                else if (currentItem.CompareTag("Openable"))
                {
                    ShowInteractUI("Openable", true);
                    Debug.Log("�� �Ǵ� ���� ����" + currentItem.name);
                }
                else if (currentItem.CompareTag("Interactable"))
                {
                    ShowInteractUI("Interactable", true);
                }
                else
                {
                    CrearCurrentItem();
                }
            }
        }
        else
        {
            CrearCurrentItem();
        }
    }

    //������Ʈ�� ���� ������ ����
    private void ShowInteractUI(string kind, bool state)
    {
        if(kind == "item")
        {
            iconImage.sprite = iconSprites[0];
        }
        else if(kind == "Openable")
        {
            iconImage.sprite = iconSprites[1];
        }
        else if(kind == "Interactable")
        {
            iconImage.sprite = iconSprites[2];
        }
        pointerIcon.SetActive(!state);
        reticleText.SetActive(state);
        iconImage.gameObject.SetActive(state);
    }

    //���� �������� �������� ���� ��� �ʱ�ȭ �ϴ� �޼ҵ�
    private void CrearCurrentItem()
    {
        if(currentItem != null)
        {
            currentItem = null;
            isReticalOnItem= false;
            pointerIcon.SetActive(true);
            reticleText.SetActive(false);
            iconImage.gameObject.SetActive(false);
        }
    }

    //��ȣ�ۿ� Ȯ��
    public void InteractionCheck()
    {
        if(currentItem != null)
        {
            Debug.Log("Ȯ��");
            if (currentItem.CompareTag("Item"))
            {
                gameManager.inventoryManager.GetItem(currentItem);
            }
            else if (currentItem.CompareTag("Openable"))
            {
                Animator _currentDoorAnim = currentItem.GetComponent<Animator>();
                DoorActive(_currentDoorAnim);
            }
            else if (currentItem.CompareTag("Interactable"))
            {
                IInteractable interactable = currentItem.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    //�ӽ� �׽�Ʈ�� �� ���� �޼ҵ�
    public void DoorActive(Animator _animator)
    {
        Vector3 directionToPlayer = (gameManager.playerController.transform.position - _animator.gameObject.transform.position).normalized;

        if(Vector3.Dot(directionToPlayer, transform.right) > 0)
        {
            _animator.SetBool("isOpen01", true);
        }
        else
        {
            _animator.SetBool("isOpen02", true);
        }
    }
}
