using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followFace : MonoBehaviour
{
    public GameObject faceDetector;
    private Vector2 faceLoc;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        faceLoc = faceDetector.GetComponent<FaceDetect>().faceLocation;
        gameObject.transform.position = new Vector3(faceLoc.x, -faceLoc.y);
    }
}
