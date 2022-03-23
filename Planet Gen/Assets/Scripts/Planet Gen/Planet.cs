using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main class for each planet that references the other scripts in order to generate the planet object with terrain
/// </summary>
public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;
    public bool autoUpdate = true;
    public enum FaceRenderMask { All, Top, Bottom, Left, Right, Front, Back };
    public FaceRenderMask faceRenderMask;

    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector]
    public bool shapeSettingsFoldOut;
    [HideInInspector]
    public bool colorSettingsFoldOut;

    ShapeGen shapeGenerator = new ShapeGen();
    ColorGen colorGenerator = new ColorGen();

    [SerializeField, HideInInspector]
    private MeshFilter[] meshFilters;
    private TerrainFace[] terrainFaces;

    /// <summary>
    /// Updates the settings of the planets and creates the mesh for the planet as well as the material
    /// </summary>
    void Initialise()
    {
        // Update the planet settings
        shapeGenerator.UpdateSettings(shapeSettings);
        colorGenerator.UpdateSettings(colorSettings);

        // Check if there are meshe filters, if not create new mesh filters
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        // create a new instance of the terrain face array
        terrainFaces = new TerrainFace[6];

        // An array of directions made so that the script can easily iterate through each direction to make a mesh for that direction
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        // for each mesh of the planet
        for (int i = 0; i < 6; i++)
        {
            // if there is a mesh filter
            if (meshFilters[i] == null)
            {
                // Create a new mesh object
                GameObject meshObj = new GameObject("Mesh");
                // Set the planet as the parent
                meshObj.transform.parent = transform;
                // Give the mesh a renderer component
                meshObj.AddComponent<MeshRenderer>();
                // Give the mesh a mesh filter so terrain can be made for it
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                // Create a new shared mesh to allow for terrain to wrap around together
                meshFilters[i].sharedMesh = new Mesh();
            }
            // Set the shared material of the mesh filter as the material in color settings
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSettings.planetMaterial;
            // Create a new terrain face using the constructed meshes
            terrainFaces[i] = new TerrainFace(shapeGenerator, meshFilters[i].sharedMesh, resolution, directions[i]);
            // Set the face to be rendered if it is selected to be rendered in the editor
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            // Set the mesh as active or inactive depending on the last line
            meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    /// <summary>
    /// Initialises the planet and then generates the mesh for it and generates the colors for it
    /// </summary>
    public void GeneratePlanet()
    {
        Initialise();
        GenerateMesh();
        GenerateColors();
    }

    /// <summary>
    /// Reinitialises the planet and re-generates the mesh of the planet when the shape settings have been changed
    /// </summary>
    public void OnShapeSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialise();
            GenerateMesh();
        }
    }

    /// <summary>
    /// Reinitialises the planet and re-generates the colors of the planet when the color settings have been changed
    /// </summary>
    public void OnColorSettingsUpdated()
    {
        if (autoUpdate)
        {
            Initialise();
            GenerateColors();
        }
    }

    /// <summary>
    /// for each terrain face that makes up the planet, this checks if their active and if they are a mesh is made out of the vertices
    /// </summary>
    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].ConstructMesh();
            }
        }

        colorGenerator.UpdateElevation(shapeGenerator.elevationMinMax);
    }

    /// <summary>
    /// Calls for the colors in the generator to be updates and then updates the meshes with the updated color generator
    /// </summary>
    void GenerateColors()
    {
        colorGenerator.UpdateColors();
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i].gameObject.activeSelf)
            {
                terrainFaces[i].UpdateUVs(colorGenerator);
            }
        }
    }
}
