using UnityEngine;
using System.Collections;

// Put this on an empty "Storm" object in Level 2.
// - Small, distant flickers happen often (quiet, dim).
// - Every so often a BIG strike flashes the whole valley and plays the main thunder sound.
// - An optional looping background rumble plays constantly.
public class Lightning : MonoBehaviour
{
    [Tooltip("The scene's Directional Light.")]
    public Light sunLight;

    [Header("Big strikes")]
    public float bigMinDelay = 15f;
    public float bigMaxDelay = 30f;
    public float bigFlashIntensity = 5f;
    [Tooltip("Main thunder sound played after a big strike.")]
    public AudioClip bigThunder;
    [Range(0f, 1f)] public float bigThunderVolume = 1f;

    [Header("Small distant flickers")]
    public float smallMinDelay = 3f;
    public float smallMaxDelay = 7f;
    public float smallFlashIntensity = 1.8f;

    [Header("Background rumble (loops constantly)")]
    public AudioClip backgroundRumble;
    [Range(0f, 1f)] public float backgroundVolume = 0.35f;

    [Header("Flash colour")]
    public Color flashColor = new Color(0.85f, 0.9f, 1f);

    private float normalIntensity;
    private Color normalColor;
    private float normalAmbient;
    private AudioSource strikeSource;
    private bool flashing;

    private void Start()
    {
        if (sunLight == null) return;
        normalIntensity = sunLight.intensity;
        normalColor = sunLight.color;
        normalAmbient = RenderSettings.ambientIntensity;

        // Source for the big thunder
        strikeSource = gameObject.AddComponent<AudioSource>();
        strikeSource.spatialBlend = 0f;
        strikeSource.playOnAwake = false;

        // Looping background rumble
        if (backgroundRumble)
        {
            AudioSource bg = gameObject.AddComponent<AudioSource>();
            bg.clip = backgroundRumble;
            bg.loop = true;
            bg.volume = backgroundVolume;
            bg.spatialBlend = 0f;
            bg.Play();
        }

        StartCoroutine(BigStrikes());
        StartCoroutine(SmallFlickers());
    }

    private IEnumerator BigStrikes()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(bigMinDelay, bigMaxDelay));

            // Two or three bright flashes close together
            int flashes = Random.Range(2, 4);
            for (int i = 0; i < flashes; i++)
            {
                yield return Flash(bigFlashIntensity, Random.Range(0.06f, 0.14f), 3f);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            }

            if (bigThunder)
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 0.6f));
                strikeSource.PlayOneShot(bigThunder, bigThunderVolume);
            }
        }
    }

    private IEnumerator SmallFlickers()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(smallMinDelay, smallMaxDelay));
            if (!flashing) yield return Flash(smallFlashIntensity, Random.Range(0.05f, 0.1f), 1.5f);
        }
    }

    private IEnumerator Flash(float intensity, float duration, float ambientMultiplier)
    {
        flashing = true;
        sunLight.intensity = intensity;
        sunLight.color = flashColor;
        RenderSettings.ambientIntensity = normalAmbient * ambientMultiplier;
        yield return new WaitForSeconds(duration);
        sunLight.intensity = normalIntensity;
        sunLight.color = normalColor;
        RenderSettings.ambientIntensity = normalAmbient;
        flashing = false;
    }
}
