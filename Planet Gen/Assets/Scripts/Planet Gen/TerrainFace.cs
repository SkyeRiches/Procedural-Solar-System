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

    public TerrainFace(ShapeGen shapeGenerator, Mesh a_mesh, int a_resolution, Vector3 a_localUp)
    {
        this.shapeGenerator = shapeGenerator;
        this.mMesh = a_mesh;
        this.iResolution = a_resolution;
        this.v3LocalUp = a_localUp;

        v3AxisA = new Vector3(v3LocalUp.y, v3LocalUp.z, v3LocalUp.x);
        v3AxisB = Vector3.Cross(v3LocalUp, v3AxisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[iResolution * iResolution];
        int[] triangles = new int[(iResolution - 1) * (iResolution - 1) * 6];
        int triIndex = 0;
        Vector2[] uv = (mMesh.uv.Length == vertices.Length) ? mMesh.uv : new Vector2[vertices.Length];

        for (int y = 0; y < iResolution; y++)
        {
            for(int x = 0; x < iResolution; x++)
            {
                int i = x + y * iResolution;
                Vector2 percent = new Vector2(x, y) / (iResolution - 1);
                Vector3 pointOnUnitCube = v3LocalUp + (percent.x - 0.5f) * 2 * v3AxisA + (percent.y - 0.5f) * 2 * v3AxisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                float unscaledElevation = shapeGenerator.CalculateUnscaledElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * shapeGenerator.GetScaledElevation(unscaledElevation);
                uv[i].y = unscaledElevation;

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
        mMesh.Clear();
        mMesh.vertices = vertices;
        mMesh.triangles = triangles;
        mMesh.RecalculateNormals();
        mMesh.uv = uv;
    }

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
