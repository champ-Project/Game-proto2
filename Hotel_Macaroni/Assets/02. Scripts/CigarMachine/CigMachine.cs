using UnityEngine;

public class CigMachine : MonoBehaviour
{
    public bool isCoinInsert = false;   //���� �־����� Ȯ���ϴ� bool ��
    private Animator animator;
    [SerializeField] private Transform itemSpawnTrans;

    public GameObject[] machineItems;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void LeverAction(int leverNum)
    {
        if (animator == null) return;
        if (isCoinInsert)
        {
            isCoinInsert = false;
            switch (leverNum)
            {
                case 1:
                    animator.SetTrigger("PullLever01");
                    break;
                case 2:
                    animator.SetTrigger("PullLever02");
                    break;
                case 3:
                    animator.SetTrigger("PullLever03");
                    if (machineItems[0] != null)
                    {
                        machineItems[0].SetActive(true);
                    }
                    break;
                case 4:
                    animator.SetTrigger("PullLever04");
                    break;
                case 5:
                    animator.SetTrigger("PullLever05");
                    break;
                default:
                    Debug.LogError("���� ��ȣ ����");
                    break;
            }
            /*if (machineItems[leverNum-1] != null)
            {
                Instantiate(machineItems[leverNum - 1], itemSpawnTrans);
            }*/
            
        }
        else
        {
            //�����ʿ�
        }

    }
}
