using SafeSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//플레이어 조준점 스크립트
//Ray, 타겟 오브젝트 저장, 오브젝트 확인 등
public class ReticleManager : MonoBehaviour
{
    public float rayDistance = 3f;          //Ray거리
    private GameObject currentItem;         //현재 조준점에 잡힌 아이템
    private bool isReticalOnItem = false;   //아이템 포착 여부 bool
    public GameObject reticleText;          //조준점 텍스트
    public GameObject pointerIcon;          //조준점 아이콘 오브젝트
    public Image iconImage;                 //아이콘 이미지 컴포넌트
    public Sprite[] iconSprites;            //아이콘 스프라이트

    private GameManager gameManager;
    [SerializeField]private PlayerInput playerInput; // PlayerInput 컴포넌트를 연결
    [SerializeField] private MonoBehaviour cameraController; // 카메라 컨트롤 스크립트 (예: FirstPersonController)


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

    //Ray로 아이템 감지, currentItem에 임시 저장
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
                    Debug.Log("아이템 포착" + currentItem.name);
                }
                else if (currentItem.CompareTag("Openable"))
                {
                    ShowInteractUI("Openable", true);
                    Debug.Log("문 또는 서랍 포착" + currentItem.name);
                }
                else if (currentItem.CompareTag("Interactable"))
                {
                    ShowInteractUI("Interactable", true);
                    Debug.Log("상호작용 오브젝트 포착" + currentItem.name );
                }
                else
                {
                    ClearCurrentItem();
                }
            }
        }
        else
        {
            ClearCurrentItem();
        }
    }

    //오브젝트에 따른 아이콘 변경
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

    //현재 조준중인 아이템이 없을 경우 초기화 하는 메소드
    private void ClearCurrentItem()
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

    //상호작용 확인
    public void InteractionCheck()
    {
        if(currentItem != null)
        {
            Debug.Log("확인");
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
                
                
            }
        }
    }

    //임시 테스트용 문 여는 메소드
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
