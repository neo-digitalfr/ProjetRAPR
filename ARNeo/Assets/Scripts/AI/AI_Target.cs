using UnityEngine;
using System.Collections;
using Vuforia;
using DarkTonic.MasterAudio;

public class AI_Target : MonoBehaviour, ITrackableEventHandler
{
    public int index;
    private Animation m_aniAnimation;

    private TrackableBehaviour mTrackableBehaviour;
    private TWEAK_EnableLight mEnableLight;

    private void Awake()
    {
        m_aniAnimation = GetComponentInChildren<Animation>();
        mEnableLight = GetComponent<TWEAK_EnableLight>();
    }

    void Start()
    {
        mTrackableBehaviour = transform.parent.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        StopAllCoroutines();
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            StartCoroutine(WaitForAnimation());
        }
        else
        {
            m_aniAnimation.Stop();
            MasterAudio.StopAllOfSound("Page" + index);
        }

        if(mEnableLight)
        {
            mEnableLight.DisableGreenLight();
            mEnableLight.DisableRedLight();
        }
    }

    private IEnumerator WaitForAnimation()
    {
        PlayAnimation();
        do
        {
            yield return null;
        } while (m_aniAnimation.isPlaying);
        StartCoroutine(WaitForAnimation());
    }

    private void PlayAnimation()
    {
        m_aniAnimation.Stop();
        m_aniAnimation.Play();
        MasterAudio.StopAllOfSound("Page" + index);
        MasterAudio.PlaySound("Page" + index);
    }

}
