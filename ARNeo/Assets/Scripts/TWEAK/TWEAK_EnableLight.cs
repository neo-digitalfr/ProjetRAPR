using UnityEngine;
using System.Collections;

public class TWEAK_EnableLight : MonoBehaviour {

    public Light m_redlight;
    public Light m_greenlight;


	public void EnableRedLight()
    {
        m_redlight.enabled = true;
    }

    public void EnableGreenLight()
    {
        m_greenlight.enabled = true;
    }

    public void DisableGreenLight()
    {
        m_greenlight.enabled = false;
    }

    public void DisableRedLight()
    {
        m_redlight.enabled = false;
    }
}
