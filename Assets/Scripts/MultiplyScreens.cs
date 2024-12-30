using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyScreens : MonoBehaviour
{
    [SerializeField] private Camera[] myCams = new Camera[2];
    void Start()
    {
        //Get Main Camera
        myCams[0] = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //Find All other Cameras
        myCams[1] = GameObject.FindGameObjectWithTag("Camera2").GetComponent<Camera>();

        //Call function when new display is connected
        Display.onDisplaysUpdated += OnDisplaysUpdated;

        //Map each Camera to a Display
        mapCameraToDisplay();
    }

    void mapCameraToDisplay()
    {
        //Loop over Connected Displays
        for (int i = 0; i < Display.displays.Length; i++)
        {
            myCams[i].targetDisplay = i; //Set the Display in which to render the camera to
            Display.displays[i].Activate(); //Enable the display
        }
    }

    void OnDisplaysUpdated()
    {
        Debug.Log("New Display Connected. Show Display Option Menu....");
    }
}