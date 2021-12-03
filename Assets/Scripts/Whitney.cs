
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

// if you see a magic number 100, its the max number of objects

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Whitney : UdonSharpBehaviour
{
    protected const int k_maxNumberOfObjects = 100;  // magic number alert! make sure to update m_numberOfObjects Range(max)

    [Header("Prefab")]

    [SerializeField] protected GameObject m_objectPrefab;

    [Header("Whitney")]

    [SerializeField] [Range(1, 100)]  // magic number alert! make sure Range(max) matches k_maxNumberOfObjects
    [UdonSynced, FieldChangeCallback(nameof(NumberOfObjects))] 
    protected int m_numberOfObjects = 50;  
    protected int NumberOfObjects { set { m_numberOfObjects = value; RequestSerialization(); } }

    [SerializeField] [Range(0.5f, 2.0f)] 
    [UdonSynced, FieldChangeCallback(nameof(CircleSize))] 
    protected float m_circleSize = 1.0f;
    protected float CircleSize { set { m_circleSize = value; RequestSerialization(); } }

    [SerializeField] [Range(0.001f, 0.1f)]
    [UdonSynced, FieldChangeCallback(nameof(SpeedScalar))] 
    protected float m_speedScaler = 0.01f;
    protected float SpeedScalar { set { m_speedScaler = value; RequestSerialization(); } }

    [SerializeField] 
    [UdonSynced, FieldChangeCallback(nameof(Is3dMode))] 
    protected bool m_3dMode = false;
    protected bool Is3dMode { set { m_3dMode = value; RequestSerialization(); } }

    [SerializeField] [Range(0.01f, 1f)]
    [UdonSynced, FieldChangeCallback(nameof(TubeSpacing))] 
    protected float m_tubeSpacing = 1.0f;
    protected float TubeSpacing { set { m_tubeSpacing = value; RequestSerialization(); } }

    [Header("Scale")]

    [SerializeField] [Range(0.001f, 1f)]
    [UdonSynced, FieldChangeCallback(nameof(GlobalScale))] 
    protected float m_globalScale = 0.25f;
    protected float GlobalScale { set { m_globalScale = value; RequestSerialization(); } }

    [SerializeField] [Range(0.1f, 1f)]
    [UdonSynced, FieldChangeCallback(nameof(BaseScaleX))] 
    protected float m_baseScaleX = 1.0f;
    protected float BaseScaleX { set { m_baseScaleX = value; RequestSerialization(); } }

    [UdonSynced, FieldChangeCallback(nameof(BaseScaleY))] 
    [SerializeField] [Range(0.1f, 1f)]
    protected float m_baseScaleY = 1.0f;
    protected float BaseScaleY { set { m_baseScaleY = value; RequestSerialization(); } }

    [UdonSynced, FieldChangeCallback(nameof(BaseScaleZ))] 
    [SerializeField] [Range(0.1f, 1f)]
    protected float m_baseScaleZ = 1.0f;
    protected float BaseScaleZ { set { m_baseScaleZ = value; RequestSerialization(); } }

    [Header("Color")]

    [SerializeField] [Range(0.001f, 1f)]
    [UdonSynced, FieldChangeCallback(nameof(ColorHueSpeed))] 
    protected float m_colorHueSpeed = 1f;
    protected float ColorHueSpeed { set { m_colorHueSpeed = value; RequestSerialization(); } }

    [SerializeField] [Range(0f, 1f)]
    [UdonSynced, FieldChangeCallback(nameof(ColorSaturation))] 
    protected float m_colorSaturation = 1f;
    protected float ColorSaturation { set { m_colorSaturation = value; RequestSerialization(); } }

    [SerializeField] [Range(0f, 1f)]
    [UdonSynced, FieldChangeCallback(nameof(ColorBrightness))] 
    protected float m_colorBrightness = 1f;
    protected float ColorBrightness { set { m_colorBrightness = value; RequestSerialization(); } }

    [SerializeField] [Range(0f, 1f)]
    [UdonSynced, FieldChangeCallback(nameof(ColorAlpha))] 
    protected float m_colorAlpha = 0.75f;
    protected float ColorAlpha { set { m_colorAlpha = value; RequestSerialization(); } }
    //[SerializeField] [Range(0f, 1f)] protected float m_metallic = 0.7f;
    //[SerializeField] [Range(0f, 1f)] protected float m_smoothness = 0.7f;

    [Header("Rotation")]

    [SerializeField] [UdonSynced, FieldChangeCallback(nameof(RotateX))] 
    protected bool m_rotateX = false;
    protected bool RotateX { set { m_rotateX = value; RequestSerialization(); } }

    [SerializeField] [UdonSynced, FieldChangeCallback(nameof(RotateY))] 
    protected bool m_rotateY = false;
    protected bool RotateY { set { m_rotateY = value; RequestSerialization(); } }

    [SerializeField] [UdonSynced, FieldChangeCallback(nameof(RotateZ))] 
    protected bool m_rotateZ = false;
    protected bool RotateZ { set { m_rotateZ = value; RequestSerialization(); } }

    [UdonSynced, FieldChangeCallback(nameof(RotationX))] 
    [SerializeField] [Range(0f, 360f)]
    protected float m_rotationX = 0.0f;
    protected float RotationX { set { m_rotationX = value; RequestSerialization(); } }

    [UdonSynced, FieldChangeCallback(nameof(RotationY))] 
    [SerializeField] [Range(0f, 360f)]
    protected float m_rotationY = 0.0f;
    protected float RotationY { set { m_rotationY = value; RequestSerialization(); } }

    [UdonSynced, FieldChangeCallback(nameof(RotationZ))] 
    [SerializeField] [Range(0f, 360f)]
    protected float m_rotationZ = 0.0f;
    protected float RotationZ { set { m_rotationZ = value; RequestSerialization(); } }

    [Header("UI Control Refs")]
    [SerializeField] protected Slider m_numberOfObjectsSlider;
    [SerializeField] protected Slider m_circleSizeSlider;
    [SerializeField] protected Slider m_speedScalerSlider;
    [SerializeField] protected Slider m_tubeLengthSlider;

    [SerializeField] protected Slider m_globalScaleSlider;
    [SerializeField] protected Slider m_xScaleSlider;
    [SerializeField] protected Slider m_yScaleSlider;
    [SerializeField] protected Slider m_zScaleSlider;

    [SerializeField] protected Slider m_colorHueSpeedSlider;
    [SerializeField] protected Slider m_colorSaturationSlider;
    [SerializeField] protected Slider m_colorBrightnessSlider;
    [SerializeField] protected Slider m_colorAlphaSlider;

    [SerializeField] protected Slider m_rotationOffsetXSlider;
    [SerializeField] protected Slider m_rotationOffsetYSlider;
    [SerializeField] protected Slider m_rotationOffsetZSlider;

    [SerializeField] protected Toggle m_rotateXToggle;
    [SerializeField] protected Toggle m_rotateYToggle;
    [SerializeField] protected Toggle m_rotateZToggle;

    [SerializeField] protected Toggle m_3dModeToggle;

    protected GameObject[] m_objects;
    protected Renderer[] m_objectRenderers;

    [UdonSynced, FieldChangeCallback(nameof(Phase))]
    protected float m_phase = 0.0f;
    protected float Phase { set { m_phase = value; RequestSerialization(); } }
    protected float m_timeSinceLastPhaseSync = 0f;
    protected const float k_timeBetweenPhaseSyncs = 10.0f;


    void Start()
    {
        m_objects = new GameObject[k_maxNumberOfObjects];
        m_objectRenderers = new Renderer[k_maxNumberOfObjects];
        for(int i = 0; i < m_objects.Length; i++)
        {
            m_objects[i] = VRCInstantiate(m_objectPrefab);
            m_objects[i].transform.SetParent(this.transform);
            m_objects[i].SetActive(false);
            m_objectRenderers[i] = m_objects[i].GetComponent<Renderer>();
        }

        m_circleSizeSlider.value = m_circleSize;
        m_speedScalerSlider.value = m_speedScaler;
        m_numberOfObjectsSlider.value = m_numberOfObjects;

        m_globalScaleSlider.value = m_globalScale;
        m_xScaleSlider.value = m_baseScaleX;
        m_yScaleSlider.value = m_baseScaleY;
        m_zScaleSlider.value = m_baseScaleZ;

        m_rotationOffsetXSlider.value = m_rotationX;
        m_rotationOffsetYSlider.value = m_rotationY;
        m_rotationOffsetZSlider.value = m_rotationZ;

        m_rotateXToggle.isOn = m_rotateX;
        m_rotateYToggle.isOn = m_rotateY;
        m_rotateZToggle.isOn = m_rotateZ;

        m_colorHueSpeedSlider.value = m_colorHueSpeed;
        m_colorSaturationSlider.value = m_colorSaturation;
        m_colorBrightnessSlider.value = m_colorBrightness;
        m_colorAlphaSlider.value = m_colorAlpha;

        m_3dModeToggle.isOn = m_3dMode;
        On3dModeToggleChanged();
        m_tubeLengthSlider.value = m_tubeSpacing;
    }
  
	private void Update()
	{
        Vector3 baseScale = new Vector3(m_baseScaleX, m_baseScaleY, m_baseScaleZ) * m_globalScale;

        for(int i = 0; i < m_objects.Length; i++)
		{
            if(i > m_numberOfObjects - 1)
			{
                m_objects[i].SetActive(false);
                continue;
			}

            m_objects[i].SetActive(true);

            float scaledPhase = m_phase * (i + 1);  // add 1 so there arent any objects that arent moving

            // position
            Vector3 position  = new Vector3(Mathf.Cos(scaledPhase) * m_circleSize, Mathf.Sin(scaledPhase) * m_circleSize, 0.0f) + this.transform.position;
            if(m_3dMode)
			{
                position.z += m_tubeSpacing * i;
			}
            m_objects[i].transform.position = position;

            // color
            Color color = Color.HSVToRGB(scaledPhase * m_colorHueSpeed % 1.0f, m_colorSaturation, m_colorBrightness);
            color.a = m_colorAlpha;
            m_objectRenderers[i].material.color = color;
            //m_objectRenderers[i].material.SetFloat("_Metallic", m_metallic);
            //m_objectRenderers[i].material.SetFloat("_Smoothness", m_smoothness);
            //m_objects[i].GetComponent<Renderer>().material.SetColor("_EmissionColor",color);

            // scale
            if(m_3dMode)
			{
                m_objects[i].transform.localScale = baseScale;
			}
            else
			{
                const float harmonicScalingOffset = 0.02f;
                m_objects[i].transform.localScale = baseScale * i * harmonicScalingOffset;
			}

            // rotation (probably dont need to modulo)
            Quaternion rotation = Quaternion.Euler(m_rotateX ? (scaledPhase * 360f + m_rotationX) % 360f : m_rotationX,
                m_rotateY ? (scaledPhase * 360f + m_rotationY) % 360f : m_rotationY,
                m_rotateZ ? (scaledPhase * 360f + m_rotationZ) % 360f : m_rotationZ);
            m_objects[i].transform.rotation = rotation;
		}

        m_phase += Time.deltaTime * m_speedScaler * (k_maxNumberOfObjects / m_numberOfObjects) ;

        m_timeSinceLastPhaseSync += Time.deltaTime;
        if(m_timeSinceLastPhaseSync > k_timeBetweenPhaseSyncs)
		{
            m_timeSinceLastPhaseSync = 0f;
            Phase = m_phase;
		}            
	}

#region Slider Functions
	public void OnNumberOfObjectsSliderChanged()
	{
        NumberOfObjects = Mathf.RoundToInt(m_numberOfObjectsSlider.value);
	}

    public void OnCircleSizeSliderChanged()
	{
        CircleSize = m_circleSizeSlider.value;
	}

    public void OnSpeedScalarSliderChanged()
	{
        SpeedScalar = m_speedScalerSlider.value;
	}

    public void OnGlobalScaleSliderChanged()
	{
        GlobalScale = m_globalScaleSlider.value;
	}

    public void OnScaleXSliderChanged()
	{
        BaseScaleX = m_xScaleSlider.value;
	}

    public void OnScaleYSliderChanged()
	{
        BaseScaleY = m_yScaleSlider.value;
	}

    public void OnScaleZSliderChanged()
	{
        BaseScaleZ = m_zScaleSlider.value;
	}

    public void OnColorSaturationSliderChanged()
	{
        ColorSaturation = m_colorSaturationSlider.value;
	}

    public void OnColorBrightnessSliderChanged()
	{
        ColorBrightness = m_colorBrightnessSlider.value;
	}

    public void OnColorAlphaSliderChanged()
	{
        ColorAlpha = m_colorAlphaSlider.value;
	}

    public void OnColorHueSpeedSliderChanged()
	{
        ColorHueSpeed = m_colorHueSpeedSlider.value;
	}

    public void OnRotationOffsetXSliderChanged()
	{
        RotationX = m_rotationOffsetXSlider.value;
	}        

    public void OnRotationOffsetYSliderChanged()
	{
        RotationY = m_rotationOffsetYSlider.value;
	}        

    public void OnRotationOffsetZSliderChanged()
	{
        RotationZ = m_rotationOffsetZSlider.value;
	}        
    public void OnTubeLengthSliderChanged()
	{
        TubeSpacing = m_tubeLengthSlider.value;
	}
#endregion

#region Toggle Functions
    public void OnRotateXToggleChanged()
	{
        RotateX = m_rotateXToggle.isOn;
	}

    public void OnRotateYToggleChanged()
	{
        RotateY = m_rotateYToggle.isOn;
	}

    public void OnRotateZToggleChanged()
	{
        RotateZ = m_rotateZToggle.isOn;
	}

    public void On3dModeToggleChanged()
	{
        Is3dMode = m_3dModeToggle.isOn;

        // TODO: move this to Update() so it doesnt get skipped on client sync
        for(int i = 0; i < m_objectRenderers.Length; i++)
        {
            m_objectRenderers[i].sortingOrder = m_3dMode ? 0 : (m_objectRenderers.Length - 1 - i);
        }
	}
#endregion

}

