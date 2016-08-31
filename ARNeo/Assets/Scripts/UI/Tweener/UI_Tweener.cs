using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;


public enum LoopType{Once,Loop,PingPong};

/// <summary>
/// User interface tweener. All the tweens herits from it, handle the value interpolation via animation curve
/// </summary>
[System.Serializable]
public class UI_Tweener : MonoBehaviour
{
	public Transform trsfTarget;
	public string m_sId="";
	public LoopType m_loopType = LoopType.Once;
	public float m_fdelay = 0f;
	public float m_fDuration = 1f;
	public bool m_bTimeScaleIndependant = false;
	public AnimationCurve m_aniCurve = CreateStraightCurve();
	public bool m_bPlayOnEnable;
	public bool m_bDeactivateAfter;
	public bool m_bDestroyAfter;
	public UnityEvent m_onEnd;
	public UnityEvent m_onStart;


	protected virtual void Awake()
	{
		//CheckInit();
	}

	/// <summary>
	/// Creates a straight curve. used by editor scripts
	/// </summary>
	/// <returns>The straight curve.</returns>
	public static AnimationCurve CreateStraightCurve()
	{
		AnimationCurve curve = new AnimationCurve();
		curve.AddKey(new Keyframe(0, 0));
		curve.AddKey(new Keyframe(1, 1));
		return curve;
	}

	protected virtual void OnEnable()
	{
		if(m_bPlayOnEnable)
		{
			StartTween();
		}
	}

	public virtual void Stop()
	{
		StopAllCoroutines();
		ValueUpdated(0f);
	}

	protected virtual void CheckInit()
	{
		if(!m_bInited)
		{
			Init();
		}
	}

	protected virtual void Init()
	{
		if(trsfTarget==null)
			m_trsfCur = transform;
		else
			m_trsfCur = trsfTarget;
		
		m_bInited = true;
		m_goCur = m_trsfCur.gameObject;

		m_curRectTransfrom = m_trsfCur.GetComponent<RectTransform>();
	}


	/// <summary>
	/// Start the tween after a delay
	/// </summary>
	/// <returns>The tween co routine.</returns>
	/// <param name="_fDelay">_f delay.</param>
	protected IEnumerator StartTweenCoRoutine(float _fDelay,bool _bResume=false)
	{
		m_fStartTime = Time.realtimeSinceStartup;
		m_fCurTime = Time.realtimeSinceStartup;
		
		while (m_fCurTime < m_fStartTime + _fDelay)
		{
			if(m_bTimeScaleIndependant)
			{
				m_fCurTime += TWEAK_IgnoreTimeScale.GetInstance().realTimeDelta;
			}
			else
			{
				m_fCurTime += Time.deltaTime;
			}
			yield return null;
		}

		ImmediateStartTween(_bResume);
	}

	protected virtual void ImmediateStartTween(bool _bResume=false)
	{
		CheckInit ();
		StartCoroutine (TweenByAnimationCurve (_bResume));
	}

	/// <summary>
	/// Get the value from the animation curve using the timer
	/// </summary>
	/// <returns>The by animation curve.</returns>
	protected IEnumerator TweenByAnimationCurve(bool _bResume=false)
	{
		if(_bResume)
		{
			m_fStartTime = Time.realtimeSinceStartup;
			m_fCurTime = m_fStartTime + m_fDuration * m_fCurAvancement;
		}
		else
		{	
			m_fStartTime = Time.realtimeSinceStartup;
			m_fCurTime = m_fStartTime;
		}

	
		while (m_fCurTime < m_fStartTime + m_fDuration)
		{
			m_fCurAvancement = Mathf.Abs(m_fLoopFactor - (m_fCurTime - m_fStartTime)/m_fDuration);
			ValueUpdated(m_aniCurve.Evaluate(m_fCurAvancement));
			if(m_bTimeScaleIndependant)
			{
				m_fCurTime += TWEAK_IgnoreTimeScale.GetInstance().realTimeDelta;
			}
			else
			{
				m_fCurTime += Time.deltaTime;
			}
			yield return null;
		}
		m_fCurAvancement = m_aniCurve.Evaluate(Mathf.Abs(m_fLoopFactor - 1f));

		ValueUpdated(m_fCurAvancement);

		switch (m_loopType) 
		{
			case LoopType.Once:
				OnTweenFinished ();
				break;
			case LoopType.Loop :
				StartCoroutine (TweenByAnimationCurve ());
				break;
			case LoopType.PingPong : 
				m_fLoopFactor = m_fLoopFactor == 0 ? 1 : 0;
				StartCoroutine (TweenByAnimationCurve ());
				break;
		}
	}

	protected virtual void ValueUpdated(float _fValue)
	{

	}

	/// <summary>
	/// Tweens in.
	/// </summary>
	public virtual void TweenIn(bool _bResume=false)
	{
		m_fLoopFactor = 0f;
		StartTween();
	}

	/// <summary>
	/// Tweens out.
	/// </summary>
	public virtual void TweenOut(bool _bResume=false)
	{
		m_fLoopFactor = 1f;
		StartTween();
	}

	protected virtual void StartTween(bool _bResume=false)
	{
		if(m_onStart != null)
		{
			m_onStart.Invoke();
		}
		StopAllCoroutines ();
		if (m_fdelay == 0f) 
		{
			ImmediateStartTween(_bResume);
		}
		else 
		{
			StartCoroutine(StartTweenCoRoutine(m_fdelay,_bResume));
		}
	}

	protected virtual void OnTweenFinished()
	{
		if(m_onEnd != null)
		{
			m_onEnd.Invoke();
		}
		if(m_bDeactivateAfter)
		{
			gameObject.SetActive(false);
		}
		if(m_bDestroyAfter)
		{
			Destroy(gameObject);
		}
	}

	private void OnDisable()
	{
		//We want to stop everything if the object is disabled
		StopAllCoroutines();
	}

	protected float m_fCurAvancement=0f;
	protected RectTransform m_curRectTransfrom;
	protected GameObject m_goCur;
	protected Transform m_trsfCur;
	protected float m_fStartTime;
	protected float m_fCurTime;
	protected float m_fLoopFactor=0f;
	protected bool m_bInited = false;
}
