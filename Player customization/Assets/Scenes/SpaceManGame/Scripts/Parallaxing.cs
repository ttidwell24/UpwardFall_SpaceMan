using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{

    public Transform[] backgrounds; // array (list) of all the back and foregrounds to be parallaxed
    private float[] parralaxScales; // the proportion of the cameras movement to move the backgrounds by
    public float smoothing = 1f; // how smooth the parralax is going to be make sure to set this above ZERO

    private Transform cam; // reference to the main camera transform
    private Vector3 previousCamPos;  // store the postition of the camera in the previous frame

    // is called before Start(). Great for refrences 
    private void Awake()
    {
        // set up the camera reference
        cam = Camera.main.transform; 
    }

    // Start is called before the first frame update
    void Start()
    {
        // the previous frame has the current frame's camera position
        previousCamPos = cam.position;

        // assigning corrisponding parralax scales 
        parralaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parralaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // for each background
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // the parralax is the opposite of the camera movement because the previous frame multiplied byt the scale
            // taking the movement and applying it to a value and mulitplying it by a scale
            float parallax = (previousCamPos.x - cam.position.x) * parralaxScales[i];

            // set a target x position which is the current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // create a target position which is the backgrounds current position with it's target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // fade between current position and target postiiotn using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);  
        }

        // set the previousCamPosition to the camera's position at the end of the frame
        previousCamPos = cam.position;

    }
}
