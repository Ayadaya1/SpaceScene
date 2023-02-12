using UnityEngine;
using AccidentalNoise;
using System.Collections.Generic;

public class Generator : MonoBehaviour {

	[SerializeField]
	int Width = 512;
	[SerializeField]
	int Height = 512;
	[SerializeField]
	int TerrainOctaves = 6;
	[SerializeField]
	double TerrainFrequency = 1.25;

	public Terrain terrain;

	public GameObject alien;

	ImplicitFractal HeightMap;
	MapData HeightData;
	Tile[,] Tiles;

	List<TileGroup> Waters = new List<TileGroup> ();
	List<TileGroup> Lands = new List<TileGroup> ();
	
	MeshRenderer HeightMapRenderer;

	void Start()
	{

		Initialize ();
		GetData (HeightMap, ref HeightData);
		//UpdateAlienPosition();
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.F5)) 
		{
			Initialize ();
			GetData (HeightMap, ref HeightData);
		}
	}

	private void Initialize()
	{
		HeightMap = new ImplicitFractal (FractalType.MULTI, 
		                               BasisType.SIMPLEX, 
		                               InterpolationType.QUINTIC, 
		                               TerrainOctaves, 
		                               TerrainFrequency, 
		                               UnityEngine.Random.Range (0, int.MaxValue));
	}

	private void GetData(ImplicitModuleBase module, ref MapData mapData)
	{
		mapData = new MapData(Width, Height);
		float min = 1;
		for (var x = 0; x < Width; x++) {
			for (var y = 0; y < Height; y++) {

				float x1 = 0, x2 = 2;
				float y1 = 0, y2 = 2;
				float dx = x2 - x1;
				float dy = y2 - y1;

				float s = x / (float)Width;
				float t = y / (float)Height;

				float nx = x1 + Mathf.Cos(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
				float ny = y1 + Mathf.Cos(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);
				float nz = x1 + Mathf.Sin(s * 2 * Mathf.PI) * dx / (2 * Mathf.PI);
				float nw = y1 + Mathf.Sin(t * 2 * Mathf.PI) * dy / (2 * Mathf.PI);

				float heightValue = (float)HeightMap.Get(nx, ny, nz, nw);

				if (heightValue > mapData.Max) mapData.Max = heightValue;
				if (heightValue < mapData.Min)
				{
					mapData.Min = heightValue;
					min = heightValue;
					Debug.Log(min);
				}
				mapData.Data[x, y] = heightValue;
            }
		}
        var mesh = new float[513, 513];
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                mesh[i, j] = -0.5f * HeightData.Data[i, j];
            }
        }
        terrain.terrainData.SetHeights(0, 0, mesh);
        terrain.Flush();
    }


    void UpdateAlienPosition()
    {
        float minHeight = float.MaxValue;
        int minX = 0, minZ = 0;

        for (int x = 0; x < terrain.terrainData.heightmapResolution; x++)
        {
            for (int z = 0; z < terrain.terrainData.heightmapResolution; z++)
            {
                float height = terrain.terrainData.GetHeight(x, z);
                if (height < minHeight)
                {
                    minHeight = height;
                    minX = x;
                    minZ = z;
                }
            }
        }

        Vector3 newPos = new Vector3(minX, 0, minZ);
        newPos.y = terrain.SampleHeight(newPos) + alien.transform.localScale.y / 2;

        alien.transform.position = newPos;
    }




}
