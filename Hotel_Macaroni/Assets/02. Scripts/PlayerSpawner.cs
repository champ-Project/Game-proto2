using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    [SerializeField]
    [Range(1, 2)] private float minHeight;

    [SerializeField] private Transform[] spawnTransforms;
    
}
