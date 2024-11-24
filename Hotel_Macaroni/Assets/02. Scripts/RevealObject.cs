using UnityEngine;

[ExecuteAlways]
public class RevealObject : MonoBehaviour
{
    [SerializeField] private Light spotLight;

    [SerializeField] private Material m_Mat;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       // m_Mat = GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (spotLight.gameObject.activeSelf == false)
        { 
            this.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            this.GetComponent<Renderer>().enabled = true;
        }
            if (m_Mat && spotLight )
        {
            if (spotLight.gameObject.activeSelf == true)
            {
                m_Mat.SetVector("_MyLightPos", spotLight.transform.position);
                m_Mat.SetVector("_MyLightDir", -spotLight.transform.forward);
            }
            
        }
    }
}
