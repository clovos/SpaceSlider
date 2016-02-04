using System;
using UnityEngine;

public class SlowPowerUpBlock : PowerUpBlock
{
	public GameObject		Target;
	public float			FadeInTime;
	public float			FadeOutTime;
	public float 			SlowAmount;

	private Vector3			m_lastTargetPos;
	private float			m_currentSlowAmount;

	public override void Start()
	{
		base.Start();
		m_type = PowerUpType.Slow;
		m_lastTargetPos = Target.transform.position;
		if(FadeInTime > FadeOutTime || FadeInTime + FadeOutTime > Duration || FadeInTime < 0 || FadeOutTime < 0 || FadeOutTime > Duration)
			Debug.LogError("Timing error in SlowPowerUpBlock!");
	}

	protected override void Update()
	{
		if(Target != null && !MapEditor.Instance)
		{
			base.Update();

			if(m_currentDuration < FadeInTime)
				m_currentSlowAmount += (SlowAmount / FadeInTime) * Time.deltaTime;
			else if(m_currentDuration < FadeOutTime)
				m_currentSlowAmount -= (SlowAmount / FadeOutTime) * Time.deltaTime;
			
			Target.transform.position -= ((Target.transform.position - m_lastTargetPos).normalized * m_currentSlowAmount) * Time.deltaTime;
		}
	}

	public override void OnCollision()
	{
		base.OnCollision();
		if(Target != null)
		{
			m_lastTargetPos = Target.transform.position;
			m_currentSlowAmount = 0;
		}
	}
}

