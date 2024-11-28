using UnityEngine;

public class OpenableSet : MonoBehaviour
{
    [SerializeField] private string objectName;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip[] clips;

    [System.Serializable]
    public struct OpenableObject
    {
        public GameObject drawerObject;
        public string clipParam;
    }

    public OpenableObject[] drawers;

    public void OpenableCheck(GameObject _currentItem)
    {
        for(int i = 0; i < drawers.Length; i++)
        {
            if (_currentItem.name == drawers[i].drawerObject.name)
            {
                ClipParamCheck(drawers[i].clipParam);
            }
        }
    }

    private void ClipParamCheck(string name)
    {
        if (!animator.GetBool(name)) //파라미터의 bool값이 false일 경우(닫혀있는 경우)
        {
            animator.SetBool(name, true);
        }
        else
        {
            animator.SetBool(name, false);
        }
    }
}
