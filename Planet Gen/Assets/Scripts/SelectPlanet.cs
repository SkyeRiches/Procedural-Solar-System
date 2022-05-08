using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPlanet : MonoBehaviour
{
    [SerializeField] private Vector3 hoverPos;
    [SerializeField] private Quaternion hoverRotation;
    [SerializeField] private GameObject manager;
    GameObject followPlanet;
    bool lockedOn;
    public bool paused;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // when the user clicks, it casts a ray out from the screen position
            Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // if it hits a planet, it will set the camera to look at the planet
            if (Physics.Raycast(ray, out hit, 500f))
            {
                if (hit.transform.tag == "Planet")
                {
                    followPlanet = hit.transform.gameObject;
                    lockedOn = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if the user presses escape, the camera will stop focusing on the current planet
            lockedOn = false;
            followPlanet = null;
            transform.position = hoverPos;
            transform.rotation = hoverRotation;
            gameObject.GetComponent<Camera>().fieldOfView = 70f;

        }

        if (lockedOn)
        {
            // camera looks at the planet to focus on
            transform.LookAt(followPlanet.transform.position, Vector3.up);
            // decrease fov to zoom in on the selected planet
            gameObject.GetComponent<Camera>().fieldOfView = 5f;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            // if the user presses 'P' it will pause/unpause the orbit visualisation
            // This is to allow for easier selection of a planet 
            // And when editing the planets it is easier if not moving too
            if (!paused)
            {
                foreach (GameObject go in manager.GetComponent<SpawnPlanet>().planets)
                {
                    go.GetComponent<PlanetMovement>().enabled = false;
                }
                paused = true;
            }
            else
            {
                foreach (GameObject go in manager.GetComponent<SpawnPlanet>().planets)
                {
                    go.GetComponent<PlanetMovement>().enabled = true;
                }
                paused = false;
            }

        }
    }
}
