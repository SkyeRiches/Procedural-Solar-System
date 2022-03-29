using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrbitPaths : MonoBehaviour
{
    [SerializeField] const float G = 300f;
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

    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsScene = simulationScene.GetPhysicsScene();
    }

    public void SimulateTrajectory(GameObject planet, Vector3 pos)
    {
        var ghostObj = Instantiate(planet, pos, planet.transform.rotation);
        ghostObj.AddComponent<GhostPlanet>();
        ghostObj.GetComponent<GhostPlanet>().SetSun(Sun);
        ghostObj.GetComponent<CelestialBody>().enabled = false;
        GhostCelestials.ghostCelestials.Add(ghostObj);

        SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);

        ghostObj.GetComponent<GhostPlanet>().NewPlanetVel(ghostObj);

        LineRenderer lineRender = Instantiate(lineRend).GetComponent<LineRenderer>();

        lineRender.positionCount = maxPhysicsFrameIter;

        for (int i = 0; i < maxPhysicsFrameIter; i ++)
        {
            ghostObj.GetComponent<GhostPlanet>().Attract(Sun.GetComponent<Rigidbody>(), ghostObj);
            physicsScene.Simulate(Time.fixedDeltaTime);
            lineRender.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }
}
