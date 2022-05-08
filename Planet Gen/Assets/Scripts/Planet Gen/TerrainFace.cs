using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    ShapeGen shapeGenerator;
    private Mesh mMesh;
    private int iResolution;
    private Vector3 v3LocalUp;
    private Vector3 v3AxisA;
    private Vector3 v3AxisB;

    /// <summary>
    /// sets the values for the terrain face based on the info passed from the shape gen and editor settings
    /// </summary>
    public TerrainFace(ShapeGen shapeGenerator, Mesh a_mesh, int a_resolution, Vector3 a_localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mMesh = a_mesh;
        this.iResolution = a_resolution;
        this.v3LocalUp = a_localUp;

        // Realigns the axis for this face to coincide with the up value passed in
        v3AxisA = new Vector3(v3LocalUp.y, v3LocalUp.z, v3LocalUp.x);
        // find the perpendicular 3rd vector using the cross product giving the terrain face a 3D axis
        v3AxisB = Vector3.Cross(v3LocalUp, v3AxisA);
    }

    /// <summary>
    /// Generates the mesh of the face based on the vertices
    /// </summary>
    public void ConstructMesh()
    {
        // calculate number of vertices
        Vector3[] vertices = new Vector3[iResolution * iResolution];
        // calculate number of triangles those vertices would make
        int[] triangles = new int[(iResolution - 1) * (iResolution - 1) * 6];

        int triIndex = 0;
        // Calculate number of uvs needed based on the length of the vertices array

        Vector2[] uv = (mMesh.uv.Length == vertices.Length) ? mMesh.uv : new Vector2[vertices.Length];

        // For each vertice that makes up the mesh
        for (int y = 0; y < iResolution; y++)
        {
            for(int x = 0; x < iResolution; x++)
            {
                int i = x + y * iResolution;

                // This tells us how close to complete each of the two for loops is, used to define where vertex should be along the face
                Vector2 percent = new Vector2(x, y) / (iResolution - 1);
                // gets the point on the face of the cube made up of the 6 faces
                Vector3 pointOnUnitCube = v3LocalUp + (percent.x - 0.5f) * 2 * v3AxisA + (percent.y - 0.5f) * 2 * v3AxisB;
                // gets the point that the previous point would be on the sphere based on the normalized value of it
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                // calculate the unscaled elevation based on the previous calculations
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                // scale the elevation and increment the point on the sphere to be scaled with that to get the true value for the vertex
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;

                // Assign the points that make up each triangle
                if (x != iResolution - 1 && y != iResolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + iResolution + 1;
                    triangles[triIndex + 2] = i + iResolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + iResolution + 1;

                    triIndex += 6;
                }
            }
        }
        // Reset all the values so that if called again this function doesnt have data in already
        mMesh.Clear();
        mMesh.vertices = vertices;
        mMesh.triangles = triangles;
        mMesh.RecalculateNormals();
        mMesh.uv = uv;
    }

    /// <summary>
    /// recalculates all the points on the uv of the mesh 
    /// </summary>
    public void UpdateUVs(ColorGen colorGen)
    {
        Vector2[] uv = mMesh.uv;

        for (int y = 0; y < iResolution; y++)
        {
            for (int x = 0; x < iResolution; x++)
            {
                int i = x + y * iResolution;
                Vector2 percent = new Vector2(x, y) / (iResolution - 1);
                Vector3 pointOnUnitCube = v3LocalUp + (percent.x - 0.5f) * 2 * v3AxisA + (percent.y - 0.5f) * 2 * v3AxisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                uv[i].x = colorGen.BiomePercentFromPoint(pointOnUnitSphere);
            }
        }
        mMesh.uv = uv;
    }
}
