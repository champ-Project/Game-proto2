using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightBlinkEvent : MonoBehaviour
{

    [SerializeField] private Light[] pointLights;
    [SerializeField] private EventManager eventManager;
    [SerializeField] private Transform playerPos;

    public Collider[] colliders;

    private bool isActive = false;

    float delayBetweenLights = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isActive)
            {
                StartCoroutine("TurnOffLights");
            }
            else
            {
                StopAllCoroutines();
                TurnOnLights();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    public void StartLightEvent()
    {
        if (!isActive)
        {
            StartCoroutine("TurnOffLights");
        }
        else
        {
            StopAllCoroutines();
            TurnOnLights();
        }
    }

    private IEnumerator TurnOffLights()
    {
        isActive = true;
        colliders[0].enabled = false;
        colliders[1].enabled = true;
        // �÷��̾�� ������ �Ÿ��� �������� ����
        List<Light> sortedLights = new List<Light>(pointLights);
        sortedLights.Sort((light1, light2) => {
            float distance1 = Vector3.Distance(playerPos.position, light1.transform.position);
            float distance2 = Vector3.Distance(playerPos.position, light2.transform.position);
            return distance2.CompareTo(distance1); // �� ������� ����
        });

        yield return new WaitForSeconds(2f);    

        // ���� ����
        foreach (Light light in sortedLights)
        {
            AudioManager.Instance.PlaySFX("SwitchOff", this.transform.position);
            light.enabled = false; // ���� ����
            yield return new WaitForSeconds(delayBetweenLights); // ���� �ð�
        }
    }

    public void TurnOnLights()
    {
        for(int i = 0; i < pointLights.Length; i++)
        {
            pointLights[i].enabled = true;
        }

        colliders[1].enabled = false;
    }
}
