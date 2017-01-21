using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]

public class BiomeType {
	public string name;
	public AudioClip clip;
	public AudioMixerGroup group;
	public AudioMixerSnapshot snapshot;
}

public class BiomeController : MonoBehaviour {
	public Leader leader;
    public Terrain terrain;
	public BiomeType[] biomes;
	private BiomeType _currentBiome;
	private float _transitionTime;

	void Start() {
        leader = leader != null ? leader : FindObjectOfType<Leader>();
        terrain = terrain != null ? terrain : FindObjectOfType<Terrain>();
        _transitionTime = 1.5f;
		foreach (BiomeType bio in biomes) {
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.clip = bio.clip;
			audioSource.loop = true;
			audioSource.outputAudioMixerGroup = bio.group;
			audioSource.Play();
		}
		FindBiome();
	}

	void Update() {
		FindBiome();
	}

	/** PRIVATE METHODS **/

	private void ChangeBiome() {
        if (_currentBiome == null) return;
		_currentBiome.snapshot.TransitionTo (_transitionTime);
	}


	private void FindBiome() {
        var currentName = _currentBiome != null ? _currentBiome.name : "nowhere";

        if (terrain == null) return;
        var terrainData = terrain.terrainData;
        var terrainPos = terrain.transform.position;
        var mapX = (int) Mathf.Round(((leader.transform.position.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
        var mapZ = (int) Mathf.Round(((leader.transform.position.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

        var alphamapData = terrain.terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
        var desert = Mathf.Max(alphamapData[0, 0, 0], alphamapData[0, 0, 2]);
        var forest = Mathf.Max(alphamapData[0, 0, 1], alphamapData[0, 0, 3]);
        var nextBiomeName = desert > forest ? "Desert" : "Forest";
        var nextBiome = GetBiome(nextBiomeName);
        if (nextBiome != null)
        {
            _currentBiome = nextBiome;
            ChangeBiome();
            Debug.Log("changed to " + nextBiomeName + " biome.");
        }
	}

	private BiomeType GetBiome(string name) {
		foreach (BiomeType bio in biomes) {
			if (bio.name == name) {
				return bio;
			}
		}
		return null;
	}
}
