using UnityEditor;
using UnityEngine;

// Adds "Tools > Particles" menu items.
// Must live in a folder called "Editor" (e.g. Assets/_Project/Editor).
// Only switches off the particle EFFECT (emission + renderer); the object itself stays visible.
public static class ParticleTools
{
    [MenuItem("Tools/Particles/Turn Off Particles Under Selected Object")]
    private static void OffUnderSelection()
    {
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("Select a parent object first (e.g. your Trees object).");
            return;
        }
        int n = SetParticles(Selection.activeGameObject.GetComponentsInChildren<ParticleSystem>(true), false);
        Debug.Log("Turned off " + n + " particle effects under " + Selection.activeGameObject.name + ".");
    }

    [MenuItem("Tools/Particles/Turn Particles Back On Under Selected Object")]
    private static void OnUnderSelection()
    {
        if (Selection.activeGameObject == null) return;
        int n = SetParticles(Selection.activeGameObject.GetComponentsInChildren<ParticleSystem>(true), true);
        Debug.Log("Turned on " + n + " particle effects.");
    }

    private static int SetParticles(ParticleSystem[] systems, bool on)
    {
        int count = 0;
        foreach (ParticleSystem ps in systems)
        {
            var psr = ps.GetComponent<ParticleSystemRenderer>();
            Undo.RecordObject(ps, "Toggle particles");
            if (psr) Undo.RecordObject(psr, "Toggle particles");

            var emission = ps.emission;
            emission.enabled = on;
            var main = ps.main;
            main.playOnAwake = on;
            if (psr) psr.enabled = on;
            if (!on) ps.Clear();

            EditorUtility.SetDirty(ps);
            count++;
        }
        return count;
    }
}
