//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
/// <summary>
/// Implements common functionality for monobehaviours that wish to have a timeScale-independent deltaTime.
/// </summary>
public class TWEAK_IgnoreTimeScale : MonoBehaviour
{
	float mTimeStart = 0f;
	float mTimeDelta = 0f;
	float mActual = 0f;
	bool mTimeStarted = false;
	private static TWEAK_IgnoreTimeScale instance;

	protected virtual void Awake()
	{
		instance = this;
	}

	public static TWEAK_IgnoreTimeScale GetInstance()
	{
		if(instance == null)
		{
			GameObject go = new GameObject();
			go.name = "TWEAK_IgnoreTimeScale";
			DontDestroyOnLoad(go);
			instance = go.AddComponent<TWEAK_IgnoreTimeScale>();
		}

		return instance;
	}

	/// <summary>
	/// Equivalent of Time.deltaTime not affected by timeScale, provided that UpdateRealTimeDelta() was called in the Update().
	/// </summary>

	public float realTimeDelta { get { return mTimeDelta; } }

	/// <summary>
	/// Clear the started flag;
	/// </summary>

	protected virtual void OnEnable ()
	{
		mTimeStarted = true;
		mTimeDelta = 0f;
		mTimeStart = Time.realtimeSinceStartup;
	}

	/// <summary>
	/// Update the 'realTimeDelta' parameter. Should be called once per frame.
	/// </summary>

	private float Update ()
	{
		if (mTimeStarted)
		{
			float time = Time.realtimeSinceStartup;
			float delta = time - mTimeStart;
			mActual += Mathf.Max(0f, delta);
			mTimeDelta = 0.001f * Mathf.Round(mActual * 1000f);
			mActual -= mTimeDelta;
			mTimeStart = time;
		}
		else
		{
			mTimeStarted = true;
			mTimeStart = Time.realtimeSinceStartup;
			mTimeDelta = 0f;
		}
		return mTimeDelta;
	}
}