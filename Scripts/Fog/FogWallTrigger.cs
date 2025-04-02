using UnityEngine;

public class FogWallTrigger : MonoBehaviour
{
    public FogHandler handler;
    public FogObscure fogObscure;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            handler.TryDamagePlayer(other.transform);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainEngine"))
        {
            fogObscure.EnterFog();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainEngine"))
        {
            fogObscure.ExitFog();
        }
    }
}
