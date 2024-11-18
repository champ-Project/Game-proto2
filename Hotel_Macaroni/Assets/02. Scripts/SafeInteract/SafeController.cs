using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

namespace SafeSystem
{
    public class SafeController : MonoBehaviour
    {
        [Header("Safe Model References")]
        [SerializeField] private GameObject safeModel = null;
        [SerializeField] private Transform safeDial = null;

        [Header("Animation Reference")]
        [SerializeField] private string safeAnimationName = "SafeDoorOpen";

        [SerializeField] private float beforeAnimationStart = 1.0f; //Default: 1.0f
        [SerializeField] private float beforeOpenDoor = 0.5f; //Default: 0.5

        [Header("Safe Solution: 0-15")]
        [Range(0, 15)][SerializeField] private int safeSolutionNum1 = 0;
        [Range(0, 15)][SerializeField] private int safeSolutionNum2 = 0;
        [Range(0, 15)][SerializeField] private int safeSolutionNum3 = 0;


        [SerializeField] private UnityEvent safeOpened = null;

        public GameObject keyObject;
        private int lockState;
        private bool canClose = false;
        private bool isInteracting = false;
        private Animator safeAnim;
        private int[] currentLockNumbers = new int[3];
        private int currentLockNumber;


        void Awake()
        {
            safeAnim = safeModel.gameObject.GetComponent<Animator>();

            for (int i = 0; i < currentLockNumbers.Length; i++)
                currentLockNumbers[i] = 0;
        }

        public void ShowSafeUI()
        {


            isInteracting = true;
            lockState = 1;
            SafeUIManager.instance.ShowMainSafeUI(true);
            DisableManager.instance.DisablePlayer(true);
            SafeUIManager.instance.SetUIButtons(this);
        }

        private void Update()
        {
            if (!canClose && isInteracting && Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSafeUI();
            }
        }

        private void CloseSafeUI()
        {
            DisableManager.instance.DisablePlayer(false);
            ResetSafeDial(false);
            SafeUIManager.instance.ShowMainSafeUI(false);
            isInteracting = false;
        }

        void ResetSafeDial(bool hasComplete)
        {
            if (!hasComplete)
            {

            }

            lockState = 1;

            SafeUIManager.instance.ResetSafeUI();
            safeDial.transform.localEulerAngles = Vector3.zero;

            // Reset the current lock number and all the current lock numbers.
            currentLockNumber = 0;
            for (int i = 0; i < currentLockNumbers.Length; i++)
            {
                currentLockNumbers[i] = 0;
            }
        }

        private IEnumerator CheckCode()
        {
            SafeUIManager.instance.PlayerInputCode();
            string safeSolution = $"{safeSolutionNum1}{safeSolutionNum2}{safeSolutionNum3}";

            if (SafeUIManager.instance.playerInputNumber == safeSolution)
            {
                DisableManager.instance.DisablePlayer(false);
                SafeUIManager.instance.ShowMainSafeUI(false);
                isInteracting = false;
                safeModel.tag = "Untagged";

                yield return new WaitForSeconds(beforeAnimationStart);
                safeAnim.Play(safeAnimationName, 0, 0.0f);
                yield return new WaitForSeconds(beforeOpenDoor);

                if (keyObject != null)
                {
                    keyObject.SetActive(true);
                }


                ResetSafeDial(true);
                safeOpened.Invoke();
            }
            else
            {
                ResetSafeDial(false);
            }
        }

        public void CheckDialNumber()
        {
            SafeUIManager.instance.ResetEventSystem();

            // Save the current lock number before switching.
            currentLockNumbers[lockState - 1] = currentLockNumber;

            if (lockState < 3)
            {
                SafeUIManager.instance.UpdateUIState(lockState);
                currentLockNumbers[lockState] = currentLockNumber;
                lockState++;
            }
            else
            {
                SafeUIManager.instance.UpdateUIState(3);
                StartCoroutine(CheckCode());
                lockState = 1;
            }

            // After switching, set the current lock number to the saved value for this lock state.
            currentLockNumber = currentLockNumbers[lockState - 1];
            SafeUIManager.instance.UpdateNumber(lockState - 1, currentLockNumber);
        }

        public void MoveDialLogic(int lockNumberSelection)
        {
            SafeUIManager.instance.ResetEventSystem();


            if (lockNumberSelection == 1 || lockNumberSelection == 3)
            {
                currentLockNumber = (currentLockNumber + 1) % 16;
                currentLockNumbers[lockState - 1] = currentLockNumber;
                RotateDial(false);
            }
            else if (lockNumberSelection == 2)
            {
                currentLockNumber = (currentLockNumber + 15) % 16;
                currentLockNumbers[lockState - 1] = currentLockNumber;
                RotateDial(true);
            }

            SafeUIManager.instance.UpdateNumber(lockState - 1, currentLockNumber);
        }

        void RotateDial(bool positive)
        {
            if (positive)
            {
                safeDial.transform.Rotate(0.0f, 0.0f, -22.5f, Space.Self);
            }
            else
            {
                safeDial.transform.Rotate(0.0f, 0.0f, 22.5f, Space.Self);
            }
        }

    }
}