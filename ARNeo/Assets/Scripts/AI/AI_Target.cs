using UnityEngine;
using System.Collections;
using Vuforia;

public class AI_Target : MonoBehaviour, ITrackableEventHandler
{
    private Animation m_aniAnimation;

    private TrackableBehaviour mTrackableBehaviour;


    private void Awake()
    {
        m_aniAnimation = GetComponentInChildren<Animation>();
    }

    void Start()
    {
        mTrackableBehaviour = transform.parent.GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    /// <summary>
    /// Implementation of the ITrackableEventHandler function called when the
    /// tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackinnnnggg");
            m_aniAnimation.Stop();
            m_aniAnimation.Play();
        }

    }
}
