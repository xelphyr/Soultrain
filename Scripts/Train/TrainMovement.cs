using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TrainMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSpeed = 180f;
    public float gap = 0.2f;

    public EngineData engine;
    public List<TrailerData> trailers = new List<TrailerData>(); // Prefabs for new trailers
    List<GameObject> trailerGOs = new List<GameObject>(); // Instantiated trailers following the train

    float countUp = 0f;
    InputAction turnAction;
    private bool startGen = false;

    void Start()
    {
        turnAction = InputSystem.actions.FindAction("Turn");
        CreateTrailers();
    }
    
    // Use FixedUpdate for physics-based movement
    void FixedUpdate()
    {
        if (engine != null)
        {
            if (trailers.Count > 0)
            {
                CreateTrailers();
            }

            Movement();
        }
    }

    void Movement()
    {
        // Read input (using fixedDeltaTime for physics updates)
        float turnValue = turnAction.ReadValue<float>() * Time.fixedDeltaTime;

        // Move the first trailer (locomotive) using Rigidbody methods
        if (trailerGOs.Count > 0)
        {
            Rigidbody rb = trailerGOs[0].GetComponent<Rigidbody>();

            // Compute the new rotation by applying a yaw rotation
            Quaternion deltaRotation = Quaternion.Euler(0f, turnValue * turnSpeed, 0f);
            Quaternion newRotation = rb.rotation * deltaRotation;
            rb.MoveRotation(newRotation);

            // Move in the new forward direction (using -speed as in your original)
            Vector3 movement = newRotation * Vector3.forward * -speed * Time.fixedDeltaTime;
            Vector3 newPosition = rb.position + movement;
            rb.MovePosition(newPosition);
        }

        // For each subsequent trailer, use the preceding trailer's marker snapshot
        if (trailerGOs.Count > 1)
        {
            for (int i = 1; i < trailerGOs.Count; i++)
            {
                MarkerManager markM = trailerGOs[i - 1].GetComponent<MarkerManager>();
                if (markM.markerList.Count > 0)
                {
                    Vector3 targetPos = markM.markerList[0].position;
                    Quaternion targetRot = markM.markerList[0].rotation;
                    Rigidbody rb = trailerGOs[i].GetComponent<Rigidbody>();
                    rb.MovePosition(targetPos);
                    rb.MoveRotation(targetRot);
                    markM.markerList.RemoveAt(0);
                }
            }
        }
    }

    void CreateTrailers()
    {
        // Create the first trailer if none exist yet.
        if (trailerGOs.Count == 0 && trailers.Count > 0)
        { 
            Debug.Log("Starting Engine production");
            GameObject temp1 = engine.MakeObject(transform.position, transform.rotation, transform);
            // Ensure MarkerManager exists
            if (!temp1.GetComponent<MarkerManager>())
            {
                temp1.AddComponent<MarkerManager>();
            }
            // Add a Rigidbody if missing and set it to kinematic
            Rigidbody rb = temp1.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = temp1.AddComponent<Rigidbody>();
            }
            rb.isKinematic = true;
            trailerGOs.Add(temp1);
            Debug.Log("Finished Engine production");
        }

        // For adding additional trailers after the first one:
        if (trailerGOs.Count > 0)
        {
            MarkerManager markM = trailerGOs[trailerGOs.Count - 1].GetComponent<MarkerManager>();
            if (countUp == 0)
            {
                markM.ClearMarkerList();
            }
            countUp += Time.fixedDeltaTime;
            if (countUp >= gap && trailers.Count > 0)
            {
                // Instantiate a new trailer at the first marker position of the last trailer
                GameObject temp = trailers[0].MakeObject(markM.markerList[0].position, markM.markerList[0].rotation, transform);
                if (!temp.GetComponent<MarkerManager>())
                {
                    temp.AddComponent<MarkerManager>();
                }
                // Add a kinematic Rigidbody if missing
                Rigidbody rbTemp = temp.GetComponent<Rigidbody>();
                if (rbTemp == null)
                {
                    rbTemp = temp.AddComponent<Rigidbody>();
                }
                rbTemp.isKinematic = true;

                trailerGOs.Add(temp);
                trailers.RemoveAt(0);
                temp.GetComponent<MarkerManager>().ClearMarkerList();
                countUp = 0f;
            }
        }
    }
}


/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TrainMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float turnSpeed = 180;
    public float gap = .2f;
    public List<GameObject> trailerList = new List<GameObject>();
    List<GameObject> trailers = new List<GameObject>();

    float countUp = 0;
    InputAction turnAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateTrailers();
        turnAction = InputSystem.actions.FindAction("Turn");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (trailerList.Count>0)
        {
            CreateTrailers();
        }
        Movement();
    
    }

    void Movement()
    {
        float turnValue = turnAction.ReadValue<float>() * Time.deltaTime;
        trailers[0].transform.Rotate(new Vector3(0f, turnValue * turnSpeed, 0f));
        trailers[0].transform.position += trailers[0].transform.forward * -speed * Time.deltaTime;

        if (trailers.Count > 1)
        {
            for (int i=1; i<trailers.Count; i++)
            {
                MarkerManager markM = trailers[i-1].GetComponent<MarkerManager>();
                trailers[i].transform.position = markM.markerList[0].position;
                trailers[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    void CreateTrailers()
    {
        if (trailers.Count == 0)
        {
            GameObject temp1 = Instantiate(trailerList[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<MarkerManager>()){
                temp1.AddComponent<MarkerManager>();
            }
            trailers.Add(temp1);
            trailerList.RemoveAt(0);
        }
        MarkerManager markM = trailers[trailers.Count-1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;
        if (countUp >= gap)
        {
            GameObject temp = Instantiate(trailerList[0], markM.markerList[0].position, markM.markerList[0].rotation, transform);
            if (!temp.GetComponent<MarkerManager>())
            {
                temp.AddComponent<MarkerManager>();
            }
            trailers.Add(temp);
            trailerList.RemoveAt(0);
            temp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }
}
*/