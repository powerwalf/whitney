
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRC.SDKBase;
using VRC.Udon;

//[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class NetworkTesting : UdonSharpBehaviour
{
    [SerializeField] Slider m_slider;

    //[UdonSynced, FieldChangeCallback(nameof(RotationSpeed))]
    protected float m_rotationSpeed = 100f;
    public float RotationSpeed {
        set
        {
            m_rotationSpeed = value;
            //RequestSerialization();
        }
        get { return m_rotationSpeed; }
	}

    //[UdonSynced, FieldChangeCallback(nameof(Phase))]
    protected float m_phase = 0.0f;
    protected float Phase
	{
        set { m_phase = value;  }
        get { return m_phase; }
	}

    protected float m_timeSinceLastPhaseSync = 0f;
    protected const float k_timeBetweenPhaseSyncs = 10.0f;

	public void OnRotationSliderChanged()
	{
        RotationSpeed = m_slider.value;
    }

	public override void OnDeserialization()
	{
        m_slider.value = m_rotationSpeed;
	}

	void Update()
    {
        transform.rotation = Quaternion.Euler(m_phase * m_rotationSpeed, m_phase * m_rotationSpeed, 0f);

        m_phase += Time.deltaTime;

        /*
        m_timeSinceLastPhaseSync += Time.deltaTime;
        if(m_timeSinceLastPhaseSync > k_timeBetweenPhaseSyncs)
		{
            m_timeSinceLastPhaseSync = 0f;
            Phase = m_phase;
		}            
        */
    }
}
