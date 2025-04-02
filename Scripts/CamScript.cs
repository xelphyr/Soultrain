using UnityEngine;

public class CamScript : MonoBehaviour
{
    public float moveSpeed;
    public GameObject engine;
    private void FixedUpdate()
    {
        if(engine==null)
        {
            engine = GameObject.Find("Engine(Clone)");
        }
        if(engine != null)
        {
            Vector3 pos = engine.transform.position;
            Vector3 newPos = new Vector3(pos.x, transform.position.y, pos.z);
            transform.position = Vector3.Lerp(transform.position, newPos, moveSpeed * Time.fixedDeltaTime * (transform.position-newPos).magnitude);
        }

    }
}
