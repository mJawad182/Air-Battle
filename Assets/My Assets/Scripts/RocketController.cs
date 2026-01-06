using UnityEngine;

public class RocketController : MonoBehaviour
{
    public static RocketController instance;
    public GameObject rockets;

    private void Awake()
    {
        instance = this;
    }
}
