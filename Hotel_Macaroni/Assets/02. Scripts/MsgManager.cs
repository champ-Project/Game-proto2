using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgManager : MonoBehaviour
{
    public GameObject subtitleUI;
    public Text subtitleText; //추후 TMP로 변환 예정

    public Image toastImage;
    public Text toastText;

    public float displayTime = 3f;

    //private Coroutine toastMsgCoroution = null;
    private Coroutine subtitleMsgCoroution = null;
    private Queue<(string, string, int, int)> toastMsgQueue = new Queue<(string, string, int, int)>();
    private Queue<(string, string, int, int)> subtitleMsgQueue = new Queue<(string, string, int, int)>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SubtitleMsg("Test");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SubtitleMsg(string content)
    {
        Debug.Log("Test1");
        subtitleMsgQueue.Enqueue(("Subtitle", content, 0, 0));
        if(subtitleMsgCoroution == null)
        {
            Debug.Log("Test2");
            subtitleMsgCoroution = StartCoroutine(ProcessMsgQueue());
        }
    }

    public void ToastMsg(string content)
    {
        toastMsgQueue.Enqueue(("Toast", content, 0, 0));
    }

    public void ToastMsg(string content, ItemData itemdata)
    {
        toastMsgQueue.Enqueue(("Toast", content, 0, 0));
    }

    private IEnumerator ProcessMsgQueue()
    {
        Debug.Log("Test3");
        while (subtitleMsgQueue.Count > 0)
        {
            var (type, content, firstParam, secondParam) = subtitleMsgQueue.Dequeue();
            if (type == "Toast")
            {

            }
            else if (type == "Subtitle")
            {
                subtitleUI.SetActive(true);
                subtitleText.text = content;
            }
            yield return new WaitForSeconds(displayTime);
            subtitleUI.SetActive(false);
        }
        subtitleMsgCoroution = null;
    }




}
