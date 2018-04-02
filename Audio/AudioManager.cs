using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Audio
{
    public string name;
    public AudioClip audio;
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;
    private static AudioManager Instance
    {
        get { return instance; }
    }

    static AudioSource musicPlayer;
    public AudioListener listener;

    [SerializeField]
    List<Audio> audioList;

    Dictionary<string, AudioSource> playingSounds;


	void Start ()
    {
        if (instance)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        //DontDestroyOnLoad(this.gameObject);
        musicPlayer = GetComponent<AudioSource>();
        listener = Camera.main.gameObject.GetComponent<AudioListener>();
    }

    private void Update()
    {
        // testbed
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            PlaySoundOnce("CARDFLIP");
        }
    }

    public void PlaySoundOnce(string name)
    {
        AudioClip clipRef = audioList.Find(x => x.name == name).audio;
        if (clipRef == null)
        {
            Debug.Log("Couldn't find audioclip of: " + name);
            return;
        }

        AudioSource audioSourceRef = this.gameObject.AddComponent<AudioSource>();
        StartCoroutine(PlaySoundOnceCoroutine(audioSourceRef, clipRef, 0.0f));
    }

    public void PlaySoundOnce(string name, float delay = 0.0f)
    {
        AudioClip clipRef = audioList.Find(x => x.name == name).audio;
        if (clipRef == null)
        {
            Debug.Log("Couldn't find audioclip of: " + name);
            return;
        }

        AudioSource audioSourceRef = this.gameObject.AddComponent<AudioSource>();
        StartCoroutine(PlaySoundOnceCoroutine(audioSourceRef, clipRef, delay));
    }

    private IEnumerator PlaySoundOnceCoroutine(AudioSource source, AudioClip clip, float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        source.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
        Destroy(source);
    }

	
    public void PlayMusic(string name, bool loop)
    {
        AudioClip clipRef = audioList.Find(x => x.name == name).audio;
        if (clipRef == null)
        {
            Debug.Log("Couldn't find audioclip of: " + name);
            return;
        }

        if (musicPlayer != null && musicPlayer.isPlaying)
            musicPlayer.Stop();

        musicPlayer.clip = clipRef;
        musicPlayer.loop = loop;
        musicPlayer.Play();
    }

    public void StopMusic()
    {
        if (musicPlayer != null && musicPlayer.isPlaying)
            musicPlayer.Stop();
    }
}
