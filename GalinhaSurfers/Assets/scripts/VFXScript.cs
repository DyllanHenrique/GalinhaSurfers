using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXScript : MonoBehaviour
{
    public VisualEffect vfx;
    public uint fixedSeed = 2; // valor fixo da seed

    void Awake()
    {
        if (!vfx)
            vfx = GetComponent<VisualEffect>();

        // trava a seed global
        vfx.resetSeedOnPlay = false;
        vfx.startSeed = fixedSeed;
    }

    void OnEnable()
    {
        if (vfx != null)
        {
            // reinicializa com a seed fixa
            vfx.Reinit();

            // dispara o evento inicial (configure o "Initial Event Name" como "ManualPlay" no Inspector)
            vfx.SendEvent("ManualPlay");
        }
    }
}
