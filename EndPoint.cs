using UnityEngine;

public class EndPoint : MonoBehaviour
{
    public InputChecker inputChecker;
    public KeyCode key;
    void Start()
    {
        for (int i = 11; i <= 14; i++)
        {
            if (i != gameObject.layer)
            {
                Physics.IgnoreLayerCollision(gameObject.layer, i);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            inputChecker.RegisterArrow(other.gameObject, key);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Arrow"))
        {
            inputChecker.UnregisterArrow(other.gameObject);
        }
    }
}