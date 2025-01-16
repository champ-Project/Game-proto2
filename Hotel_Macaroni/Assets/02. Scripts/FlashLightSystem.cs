using UnityEngine;

public class FlashLightSystem : MonoBehaviour
{
    [SerializeField] private Vector3 offSet;
    [SerializeField] private Transform goFollow;
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private Transform player;
    [SerializeField] private Transform flashlight;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offSet = transform.position - goFollow.transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.position = goFollow.transform.position + offSet;
        transform.rotation = Quaternion.Lerp(transform.rotation, goFollow.transform.rotation, speed * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, goFollow.rotation, speed * Time.deltaTime);

        //Quaternion targetRotation = player.rotation;
        //flashlight.rotation = Quaternion.Slerp(flashlight.rotation, targetRotation, speed * Time.deltaTime);
    }
}
