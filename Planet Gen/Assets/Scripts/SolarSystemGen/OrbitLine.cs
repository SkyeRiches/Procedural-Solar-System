using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbitLine : MonoBehaviour
{
    private Scene simulationScene;
    private PhysicsScene physicsScene;
    public GameObject Sun;
    [SerializeField] private Material lineMat;
    [SerializeField] private int maxPhysicsFrameIter = 10000;
    [SerializeField] private GameObject lineRend;

    
    private void Start()
    {
        CreatePhysicsScene();
    }

    // Create a new physics scene that will be used to calculate the orbit path of each planet by running the orbit at an increased pace.
    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsScene = simulationScene.GetPhysicsScene();
    }

    public void SimulateTrajectory(GameObject planet, Vector3 pos, float a_fSpeed)
    {
        // create a copy of the planet just spawned
        var ghostObj = Instantiate(planet, pos, planet.transform.rotation);
        // move the copy to the physics scene
        SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
        // create a new line render
        LineRenderer lineRender = Instantiate(lineRend, ghostObj.transform.position, Quaternion.identity).GetComponent<LineRenderer>();

        float orbitCompletion = 0;

        // while it has not completed a full 360 degree loop
        while (orbitCompletion <= 360)
        {
            // rotate planet around the star
            ghostObj.transform.RotateAround(Sun.transform.position, Vector3.up, a_fSpeed * Time.deltaTime);
            // add distance done to the orbit completion
            orbitCompletion += a_fSpeed * Time.deltaTime;
            // Increment the simulation
            physicsScene.Simulate(Time.fixedDeltaTime);
            // add another position to the line render
            lineRender.positionCount++;
            lineRender.SetPosition(lineRender.positionCount - 1, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }

}
