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
	public BiomeType[] biomes;
	private BiomeType _currentBiome;
	private float _transitionTime;

	void Start() {
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
        RaycastHit hit;
		if (Physics.Raycast (leader.transform.position + Vector3.up, Vector3.down, out hit)) {
			var tag = hit.collider.gameObject.tag;
			if (tag != currentName) {
                var nextBiome = GetBiome(tag);
                if (nextBiome != null)
                {
                    _currentBiome = nextBiome;
                    ChangeBiome();
                    Debug.Log("changed to " + tag + " biome.");
                }
			}
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
