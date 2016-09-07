/*
using UnityEngine;
using System.Collections;
using UnityEditor.Events;
using UnityEngine.UI;
*/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class MovieMgr : MonoBehaviour
{
    public MovieTexture movieclips;
    private MeshRenderer meshRenderer;
    private bool trigger;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = movieclips;

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        movieclips.Play();
        trigger = false;
    }

    void Update()
    {
        if (!movieclips.isPlaying && !trigger)
        {
            trigger = true;
            Application.LoadLevel("LoadingScene");
        }
            
    }

}

/*
[RequireComponent(typeof(AudioSource))]
public class MovieMgr : MonoBehaviour
{
    Object[] movie_stills;
    int number_of_stills = 0;
    public bool loop = false;
    public bool playOnStart = false;
    public int fps = 30;
    public AudioClip sound;
    public string resourceSubfolder = "";

    private int stills = 0;
    private bool play = false;
    private bool loaded = false;
    private new AudioSource audio;
    void Update()
    {

        if (!resourceSubfolder.Equals("") && !loaded) {
            StartCoroutine(ImportVideo());
        }

        if (fps > 0)
        {
            if (play == true)
            {
                StartCoroutine(Player());
            }
        }
        else
        {
            Debug.LogError("'fps' must be set to a value greater than 0.");
        }
    }

    IEnumerator Player()
    {
        play = false;
        if (loop)
        {
            Debug.Log("looped. stills: " + stills + ", length: " + movie_stills.Length);
            if (stills >= movie_stills.Length)
            {
                audio.Stop();
                audio.clip = sound;
                audio.Play();
                stills = 0;
                Debug.Log("restarting. stills: " + stills);
            }
        }
        else
        {
            if (stills > movie_stills.Length)
            {
                audio.Stop();
                stills -= 1;
            }
        }

        if (stills >= 0 && stills < movie_stills.Length) {
            Texture2D MainTex = movie_stills[stills] as Texture2D;
            GetComponent<Renderer>().material.SetTexture("_MainTex", MainTex);
            stills += 1;
            int fps_fixer = fps * 3;
            float wait_time = 1.0f / fps_fixer;
            yield return new WaitForSeconds(wait_time);
            if (!audio.clip)
            {
                if (sound)
                {
                    audio.clip = sound;
                    audio.Play();
                }
            }
            play = true;
        }
    }

    public void Play() { play = true; }
    public void Pause() { play = false; }

    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.loop = false;
        number_of_stills -= 1;
    }


    IEnumerator ImportVideo()
    {
        movie_stills = Resources.LoadAll(resourceSubfolder, typeof(Texture2D));
        loaded = true;
        if (playOnStart)
            play = true;
        yield return null;
    }

    public void UnloadFromMemory()
    {
        play = false;
        audio.Stop();

        foreach (Object o in movie_stills) Destroy(o);
    }
}
*/

