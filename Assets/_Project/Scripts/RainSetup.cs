using UnityEngine;

// Put this on the "Rain" object (child of PlayerArmature) that has a Particle System.
// When the game starts it configures the particle system as falling rain,
// so you don't have to set every option by hand.
[RequireComponent(typeof(ParticleSystem))]
public class RainSetup : MonoBehaviour
{
    [Header("Rain settings")]
    public float areaSize = 40f;        // width/length of the rain area around the player
    public float heightAbovePlayer = 15f;
    public float dropsPerSecond = 1000f;
    public float fallSpeed = 25f;
    public float dropSize = 0.06f;
    public Color dropColor = new Color(0.85f, 0.9f, 1f, 0.6f);

    private void Awake()
    {
        // Position the emitter above the player, flat, pointing down
        transform.localPosition = new Vector3(0f, heightAbovePlayer, 0f);
        transform.localRotation = Quaternion.Euler(90f, 0f, 0f); // emits along local Z = world down
        transform.localScale = Vector3.one;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main = ps.main;
        main.loop = true;
        main.playOnAwake = true;
        main.duration = 5f;
        main.startLifetime = (heightAbovePlayer + 5f) / fallSpeed;
        main.startSpeed = fallSpeed;
        main.startSize = dropSize;
        main.startColor = dropColor;
        main.gravityModifier = 0f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles = 10000;
        main.scalingMode = ParticleSystemScalingMode.Local;

        var emission = ps.emission;
        emission.enabled = true;
        emission.rateOverTime = dropsPerSecond;

        var shape = ps.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(areaSize, areaSize, 0.1f);
        shape.position = Vector3.zero;
        shape.rotation = Vector3.zero;

        var renderer = GetComponent<ParticleSystemRenderer>();
        renderer.enabled = true;   // make sure the Renderer module is switched on
        renderer.renderMode = ParticleSystemRenderMode.Stretch;
        renderer.lengthScale = 4f;
        renderer.velocityScale = 0f;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        // Make sure there is a visible URP particle material
        if (renderer.sharedMaterial == null || renderer.sharedMaterial.shader == null ||
            !renderer.sharedMaterial.shader.name.Contains("Particles"))
        {
            Shader sh = Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if (sh != null)
            {
                Material m = new Material(sh);
                m.SetFloat("_Surface", 1f); // transparent
                m.SetColor("_BaseColor", Color.white);
                renderer.material = m;
            }
        }

        ps.Play();
        Debug.Log("[Rain] Started. Material: " + (renderer.sharedMaterial ? renderer.sharedMaterial.name + " / " + renderer.sharedMaterial.shader.name : "NONE")
                  + ", layer: " + LayerMask.LayerToName(gameObject.layer) + " (" + gameObject.layer + ")");
        InvokeRepeating(nameof(Report), 2f, 3f);
    }

    private void Report()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        Camera cam = Camera.main;
        bool camSees = cam != null && (cam.cullingMask & (1 << gameObject.layer)) != 0;
        Debug.Log("[Rain] Live particles: " + ps.particleCount + ", emitting: " + ps.emission.enabled
                  + ", main camera can see this layer: " + camSees);
    }
}
