using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerationEditor : EditorWindow
{
    Color color;
    bool showTerSettings = false;
    bool showColorSettings = false;

    int index = 0;

    private List<bool> noiseLayers = new List<bool>();
    private List<bool> biomes = new List<bool>();

    private Vector2 scrollPos;

    [MenuItem("Window/PLANET_SETTINGS")]
    public static void ShowWindow()
    {
        // created the window
        GetWindow<GenerationEditor>("Planet Settings");
    }

    public void OnSelectionChange()
    {
        // clears the list when a planet is deselected, so as to not have incorrect values
        noiseLayers.Clear();
        biomes.Clear();
    }

    private void OnGUI()
    {
        // TITLE label
        EditorGUILayout.LabelField("PLANET SETTINGS");

        // if no planet is selected, it doesnt need to bother with the rest of the code
        if (Selection.activeGameObject == null)
        {
            return;
        }

        if (Selection.activeGameObject.tag == "Planet")
        {
            GameObject go = Selection.activeGameObject;

            // add to the noise layers and biomes list for each of them that exist on the current selected planet
            while (noiseLayers.Count < go.GetComponent<ShapeSettings>().noiseLayers.Length)
            {
                noiseLayers.Add(false);
            }
            while (biomes.Count < go.GetComponent<ColorSettings>().biomeColorSettings.biomes.Length)
            {
                biomes.Add(false);
            }

            // allow scrolling on the window
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            #region BASIC_SETTINGS
            EditorGUILayout.Space(10f);

            // button that links to the generate planet function in the planet script
            if (GUILayout.Button("Apply Settings"))
            {
                if (Selection.activeGameObject.tag == "Planet")
                {
                    Selection.activeGameObject.GetComponent<Planet>().GeneratePlanet();
                }
            }

            EditorGUILayout.Space(25f);

            // adds a slider for the resolution of be modified between 0 and 256
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Resolution");
            go.GetComponent<Planet>().resolution = (int)EditorGUILayout.Slider(go.GetComponent<Planet>().resolution, 0, 256);
            EditorGUILayout.EndHorizontal();

            // Drop down option to select which faces are rendered 
            EditorGUILayout.BeginHorizontal();
            go.GetComponent<Planet>().faceRenderMask = (Planet.FaceRenderMask)EditorGUILayout.EnumPopup("Rendered-Face", go.GetComponent<Planet>().faceRenderMask);
            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.Space(10f);

            #region TERRAIN_SETTINGS
            // creates a collapsable section for the terrain settings
            showTerSettings = EditorGUILayout.Foldout(showTerSettings, "TERRAIN SETTINGS", true);
            EditorGUI.indentLevel++;

            if (showTerSettings)
            {
                go.GetComponent<ShapeSettings>().planetRadius = EditorGUILayout.FloatField("Radius", go.GetComponent<ShapeSettings>().planetRadius);
                EditorGUILayout.Space(10f);

                // uses the noise layers list to loop through each noise layer and display the settings for the layer
                for (int i = 0; i < go.GetComponent<ShapeSettings>().noiseLayers.Length; i++)
                {
                    noiseLayers[i] = EditorGUILayout.Foldout(noiseLayers[i], "Noise Layer " + i.ToString(), true);

                    NoiseSettings noiseSettings = go.GetComponent<ShapeSettings>().noiseLayers[i].noiseSettings;

                    if (noiseLayers[i])
                    {
                        // tickbox option for if the layer should be enabled
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(50f);
                        GUILayout.Label("Enabled");
                        go.GetComponent<ShapeSettings>().noiseLayers[i].enabled = EditorGUILayout.Toggle(go.GetComponent<ShapeSettings>().noiseLayers[i].enabled);
                        EditorGUILayout.EndHorizontal();

                        // tick box option for if the first layer should be used as a mask for this layer (values get added ontop so first layer is minimum setting)
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(50f);
                        GUILayout.Label("Use first layer as mask");
                        go.GetComponent<ShapeSettings>().noiseLayers[i].useFirstLayerAsMask = EditorGUILayout.Toggle(go.GetComponent<ShapeSettings>().noiseLayers[i].useFirstLayerAsMask);
                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(5f);

                        // Label for the noise settings
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(50f);
                        GUILayout.Label("Noise Settings", EditorStyles.boldLabel);
                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(5f);

                        // Drop down option for selecting what type of noise it is (rigid/simple)
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(50f);
                        noiseSettings.filterType = (NoiseSettings.FilterType)EditorGUILayout.EnumPopup("Filter Type", noiseSettings.filterType);
                        EditorGUILayout.EndHorizontal();

                        if (noiseSettings.filterType == (NoiseSettings.FilterType.Simple))
                        {
                            // Slider for the strength of the noise, higher value produces more noise 
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Strength");
                            if (i == 0)
                            {
                                noiseSettings.simpleNoiseSettings.strength = EditorGUILayout.Slider(noiseSettings.simpleNoiseSettings.strength, 0.01f, 0.2f);
                            }
                            else
                            {
                                noiseSettings.simpleNoiseSettings.strength = EditorGUILayout.Slider(noiseSettings.simpleNoiseSettings.strength, 0.01f, 5f);
                            }
                            EditorGUILayout.EndHorizontal();

                            // slider for the number of layers to be used for this noise layer
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Number of Layers");
                            noiseSettings.simpleNoiseSettings.numLayers = (int)EditorGUILayout.Slider(noiseSettings.simpleNoiseSettings.numLayers, 0, 8);
                            EditorGUILayout.EndHorizontal();

                            // Slider for the base roughness of this layer (minimum value)
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Base Roughness");
                            if (i == 0)
                            {
                                noiseSettings.simpleNoiseSettings.baseRoughness = EditorGUILayout.Slider(noiseSettings.simpleNoiseSettings.baseRoughness, 0.4f, 2f);
                            }
                            else
                            {
                                noiseSettings.simpleNoiseSettings.baseRoughness = EditorGUILayout.Slider(noiseSettings.simpleNoiseSettings.baseRoughness, 0.5f, 5f);
                            }
                            EditorGUILayout.EndHorizontal();

                            // Slider for additional roughness to be added ontop of the base roughness
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Roughness");
                            if (i == 0)
                            {
                                noiseSettings.simpleNoiseSettings.roughness = EditorGUILayout.Slider(noiseSettings.simpleNoiseSettings.roughness, 0.4f, 2f);
                            }
                            else
                            {
                                noiseSettings.simpleNoiseSettings.roughness = EditorGUILayout.Slider(noiseSettings.simpleNoiseSettings.roughness, 0.5f, 5f);
                            }
                            EditorGUILayout.EndHorizontal();

                            // Slider for the persistence setting (higher value makes it apply smoother around the planet, lower value makes it more patchy)
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Persistence");
                            noiseSettings.simpleNoiseSettings.persistence = EditorGUILayout.Slider(noiseSettings.simpleNoiseSettings.persistence, 0.1f, 0.5f);
                            EditorGUILayout.EndHorizontal();

                            // Vector 3 field where the centre of the terrain is on the planet, adjusting this changes where the origin of the noise is, thus changing the planets landmass positioning
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(50f);
                            noiseSettings.simpleNoiseSettings.centre = EditorGUILayout.Vector3Field("Centre", noiseSettings.simpleNoiseSettings.centre);
                            EditorGUILayout.EndHorizontal();
                        }
                        else if (noiseSettings.filterType == (NoiseSettings.FilterType.Rigid))
                        {
                            // Slider for the strength of the noise, higher value produces more noise 
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Strength");
                            if (i == 0)
                            {
                                noiseSettings.rigidNoiseSettings.strength = EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.strength, 0.01f, 0.2f);
                            }
                            else
                            {
                                noiseSettings.rigidNoiseSettings.strength = EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.strength, 0.01f, 5f);
                            }
                            EditorGUILayout.EndHorizontal();

                            // slider for the number of layers to be used for this noise layer
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Number of Layers");
                            noiseSettings.rigidNoiseSettings.numLayers = (int)EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.numLayers, 0, 8);
                            EditorGUILayout.EndHorizontal();

                            // Slider for the base roughness of this layer (minimum value)
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Base Roughness");
                            if (i == 0)
                            {
                                noiseSettings.rigidNoiseSettings.baseRoughness = EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.baseRoughness, 0.4f, 2f);
                            }
                            else
                            {
                                noiseSettings.rigidNoiseSettings.baseRoughness = EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.baseRoughness, 0.5f, 5f);
                            }
                            EditorGUILayout.EndHorizontal();

                            // Slider for additional roughness to be added ontop of the base roughness
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Roughness");
                            if (i == 0)
                            {
                                noiseSettings.rigidNoiseSettings.roughness = EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.roughness, 0.4f, 2f);
                            }
                            else
                            {
                                noiseSettings.rigidNoiseSettings.roughness = EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.roughness, 0.5f, 5f);
                            }
                            EditorGUILayout.EndHorizontal();

                            // Slider for the persistence setting (higher value makes it apply smoother around the planet, lower value makes it more patchy)
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Persistence");
                            noiseSettings.rigidNoiseSettings.persistence = EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.persistence, 0.1f, 0.5f);
                            EditorGUILayout.EndHorizontal();

                            // Vector 3 field where the centre of the terrain is on the planet, adjusting this changes where the origin of the noise is, thus changing the planets landmass positioning
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(50f);
                            noiseSettings.rigidNoiseSettings.centre = EditorGUILayout.Vector3Field("Centre", noiseSettings.rigidNoiseSettings.centre);
                            EditorGUILayout.EndHorizontal();

                            // slider for how much of an influence the rigid noise layer has on the terrain generation
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Space(64f);
                            GUILayout.Label("Weight Multiplier");
                            noiseSettings.rigidNoiseSettings.weightMultiplier = EditorGUILayout.Slider(noiseSettings.rigidNoiseSettings.weightMultiplier, 0, 1);
                            EditorGUILayout.EndHorizontal();
                        }

                        // Button to remove the layer from the terrain
                        if (GUILayout.Button("Remove Layer"))
                        {
                            ShapeSettings.NoiseLayer[] noiseLayers = new ShapeSettings.NoiseLayer[go.GetComponent<ShapeSettings>().noiseLayers.Length - 1];
                            // shifts the elements of the list along
                            for (int j = i; j < go.GetComponent<ShapeSettings>().noiseLayers.Length - 1; j++)
                            {
                                go.GetComponent<ShapeSettings>().noiseLayers[j] = go.GetComponent<ShapeSettings>().noiseLayers[j + 1];
                            }
                            // copies the elements into a duplicate list that is shorter
                            for (int k = 0; k < noiseLayers.Length; k++)
                            {
                                noiseLayers[k] = go.GetComponent<ShapeSettings>().noiseLayers[k];
                            }
                            // changes the list in the shape settings to the new one
                            go.GetComponent<ShapeSettings>().noiseLayers = noiseLayers;
                        }
                    }

                    
                    GUILayout.Space(10f);
                }

                // Button to add a new noise layer
                if (GUILayout.Button("Add Noise Layer"))
                {
                    // creates duplicate array of noise layers that is one bigger and adds the new noise layer to it
                    ShapeSettings.NoiseLayer[] noiseLayers = new ShapeSettings.NoiseLayer[go.GetComponent<ShapeSettings>().noiseLayers.Length + 1];
                    go.GetComponent<ShapeSettings>().noiseLayers.CopyTo(noiseLayers, 0);
                    go.GetComponent<ShapeSettings>().noiseLayers = noiseLayers;
                    // Initialise the noise layer and noise settings
                    go.GetComponent<ShapeSettings>().noiseLayers[go.GetComponent<ShapeSettings>().noiseLayers.Length - 1] = new ShapeSettings.NoiseLayer();
                    go.GetComponent<ShapeSettings>().noiseLayers[go.GetComponent<ShapeSettings>().noiseLayers.Length - 1].noiseSettings = new NoiseSettings();
                    if (go.GetComponent<ShapeSettings>().noiseLayers[go.GetComponent<ShapeSettings>().noiseLayers.Length - 1].noiseSettings.filterType == NoiseSettings.FilterType.Simple)
                    {
                        go.GetComponent<ShapeSettings>().noiseLayers[go.GetComponent<ShapeSettings>().noiseLayers.Length - 1].noiseSettings.simpleNoiseSettings = new NoiseSettings.SimpleNoiseSettings();
                    }
                    else if (go.GetComponent<ShapeSettings>().noiseLayers[go.GetComponent<ShapeSettings>().noiseLayers.Length - 1].noiseSettings.filterType == NoiseSettings.FilterType.Rigid)
                    {
                        go.GetComponent<ShapeSettings>().noiseLayers[go.GetComponent<ShapeSettings>().noiseLayers.Length - 1].noiseSettings.rigidNoiseSettings = new NoiseSettings.RigidNoiseSettings();
                    }

                }
            }

            EditorGUI.indentLevel--;
            #endregion

            EditorGUILayout.Space(25f);

            #region COLOR_SETTINGS
            // Collapsable section for the color settings
            showColorSettings = EditorGUILayout.Foldout(showColorSettings, "COLOR SETTINGS", true, EditorStyles.foldout);
            EditorGUI.indentLevel++;

            if (showColorSettings)
            {
                // loops through all biomes
                for (int i = 0; i < go.GetComponent<ColorSettings>().biomeColorSettings.biomes.Length; i++)
                {
                    biomes[i] = EditorGUILayout.Foldout(biomes[i], "Biome " + i.ToString(), true);

                    if (biomes[i])
                    {
                        // Gradient field for modifying the color gradient of the biome
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(50f);
                        GUILayout.Label("Gradient");
                        go.GetComponent<ColorSettings>().biomeColorSettings.biomes[i].gradient = EditorGUILayout.GradientField(go.GetComponent<ColorSettings>().biomeColorSettings.biomes[i].gradient);
                        EditorGUILayout.EndHorizontal();

                        // Color field for the tint of the planet
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(50f);
                        GUILayout.Label("Tint");
                        go.GetComponent<ColorSettings>().biomeColorSettings.biomes[i].tint = EditorGUILayout.ColorField(go.GetComponent<ColorSettings>().biomeColorSettings.biomes[i].tint);
                        EditorGUILayout.EndHorizontal();

                        // Slider for the start height of the biome, this is the point at which the coloration starts applying to the planet
                        // allows for creation of bands around the planet which have different climates
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(50f);
                        GUILayout.Label("Start Height");
                        go.GetComponent<ColorSettings>().biomeColorSettings.biomes[i].startHeight = EditorGUILayout.Slider(go.GetComponent<ColorSettings>().biomeColorSettings.biomes[i].startHeight, 0, 1);
                        EditorGUILayout.EndHorizontal();

                        // slider for how strong the tint is
                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(50f);
                        GUILayout.Label("Tint Percentage");
                        go.GetComponent<ColorSettings>().biomeColorSettings.biomes[i].tintPercent = EditorGUILayout.Slider(go.GetComponent<ColorSettings>().biomeColorSettings.biomes[i].tintPercent, 0, 1);
                        EditorGUILayout.EndHorizontal();

                        // Remove biome button
                        if (GUILayout.Button("Remove Biome"))
                        {
                            ColorSettings.BiomeColorSettings.Biome[] biomes = new ColorSettings.BiomeColorSettings.Biome[go.GetComponent<ColorSettings>().biomeColorSettings.biomes.Length - 1];
                            // shifts the elements of the list along
                            for (int j = i; j < go.GetComponent<ColorSettings>().biomeColorSettings.biomes.Length - 1; j++)
                            {
                                go.GetComponent<ColorSettings>().biomeColorSettings.biomes[j] = go.GetComponent<ColorSettings>().biomeColorSettings.biomes[j + 1];
                            }
                            // copies the elements into a duplicate list that is shorter
                            for (int k = 0; k < biomes.Length; k++)
                            {
                                biomes[k] = go.GetComponent<ColorSettings>().biomeColorSettings.biomes[k];
                            }
                            // replaces the biome list in the color settings with the new one
                            go.GetComponent<ColorSettings>().biomeColorSettings.biomes = biomes;
                        }
                    }

                    GUILayout.Space(10f);

                }

                // button to add a new biome to the planet
                if (GUILayout.Button("Add Biome"))
                {
                    // creates duplicate biome array that is one element longer
                    ColorSettings.BiomeColorSettings.Biome[] biomes = new ColorSettings.BiomeColorSettings.Biome[go.GetComponent<ColorSettings>().biomeColorSettings.biomes.Length + 1];
                    go.GetComponent<ColorSettings>().biomeColorSettings.biomes.CopyTo(biomes, 0);
                    go.GetComponent<ColorSettings>().biomeColorSettings.biomes = biomes;
                    // Initialise the biome settings
                    go.GetComponent<ColorSettings>().biomeColorSettings.biomes[go.GetComponent<ColorSettings>().biomeColorSettings.biomes.Length - 1] = new ColorSettings.BiomeColorSettings.Biome();
                    go.GetComponent<ColorSettings>().biomeColorSettings.biomes[go.GetComponent<ColorSettings>().biomeColorSettings.biomes.Length - 1].gradient = new Gradient();
                }

                // Slider for how blended the biomes are to create a smooth transition between them
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25f);
                GUILayout.Label("Blend Percent");
                go.GetComponent<ColorSettings>().biomeColorSettings.blendAmount = EditorGUILayout.Slider(go.GetComponent<ColorSettings>().biomeColorSettings.blendAmount, 0, 1);
                EditorGUILayout.EndHorizontal();

                // Gradient field for the color of the ocean so you can customise how the ocean looks
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(25f);
                GUILayout.Label("Ocean Color");
                go.GetComponent<ColorSettings>().oceanColor = EditorGUILayout.GradientField(go.GetComponent<ColorSettings>().oceanColor);
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10f);
            }
            

            EditorGUI.indentLevel--;
            #endregion

            EditorGUILayout.Space(25f);
            EditorGUILayout.EndScrollView();
        }
    }
}

