﻿
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

// if you see a magic number 100, its the max number of objects

public class Whitney : UdonSharpBehaviour
{
    protected const int k_maxNumberOfObjects = 100;  // magic number alert! make sure to update m_numberOfObjects Range(max)

    [Header("Prefab")]
    [SerializeField] protected GameObject m_objectPrefab;

    [Header("Whitney")]
    [SerializeField] [Range(1, 100)] protected int m_numberOfObjects = 50;  // magic number alert! make sure Range(max) matches k_maxNumberOfObjects
    [SerializeField] protected float m_circleSize = 1.0f;
    [SerializeField] [Range(0.001f, 0.1f)] protected float m_speedScaler = 0.01f;
    [SerializeField] protected bool m_3dMode = false;
    [SerializeField] [Range(0.01f, 1f)] protected float m_tubeSpacing = 1.0f;

    [Header("Scale")]
    [SerializeField] [Range(0.001f, 1f)] protected float m_globalScale = 0.25f;
    [SerializeField] [Range(0.1f, 1f)] protected float m_baseScaleX = 1.0f;
    [SerializeField] [Range(0.1f, 1f)] protected float m_baseScaleY = 1.0f;
    [SerializeField] [Range(0.1f, 1f)] protected float m_baseScaleZ = 1.0f;

    [Header("Color")]
    [SerializeField] [Range(0.001f, 1f)] protected float m_colorHueSpeed = 1f;
    [SerializeField] [Range(0f, 1f)] protected float m_colorSaturation = 0.7f;
    [SerializeField] [Range(0f, 1f)] protected float m_colorBrightness = 0.7f;
    [SerializeField] [Range(0f, 1f)] protected float m_colorAlpha = 0.7f;

    [Header("Rotation")]
    [SerializeField] protected bool m_rotateX = false;
    [SerializeField] protected bool m_rotateY = false;
    [SerializeField] protected bool m_rotateZ = false;
    [SerializeField] [Range(0f, 360f)] protected float m_rotationX = 0.0f;
    [SerializeField] [Range(0f, 360f)] protected float m_rotationY = 0.0f;
    [SerializeField] [Range(0f, 360f)] protected float m_rotationZ = 0.0f;

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
    protected float m_phase = 0.0f;


    void Start()
    {
        m_objects = new GameObject[k_maxNumberOfObjects];
        for(int i = 0; i < m_objects.Length; i++)
        {
            m_objects[i] = VRCInstantiate(m_objectPrefab);
            m_objects[i].transform.SetParent(this.transform);
            m_objects[i].SetActive(false);
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
            m_objects[i].GetComponent<Renderer>().material.color = color;

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
	}

#region Slider Functions
	public void OnNumberOfObjectsSliderChanged()
	{
        m_numberOfObjects = Mathf.RoundToInt(m_numberOfObjectsSlider.value);
	}

    public void OnCircleSizeSliderChanged()
	{
        m_circleSize = m_circleSizeSlider.value;
	}

    public void OnSpeedScalarSliderChanged()
	{
        m_speedScaler = m_speedScalerSlider.value;
	}

    public void OnGlobalScaleSliderChanged()
	{
        m_globalScale = m_globalScaleSlider.value;
	}

    public void OnScaleXSliderChanged()
	{
        m_baseScaleX = m_xScaleSlider.value;
	}

    public void OnScaleYSliderChanged()
	{
        m_baseScaleY = m_yScaleSlider.value;
	}

    public void OnScaleZSliderChanged()
	{
        m_baseScaleZ = m_zScaleSlider.value;
	}

    public void OnColorSaturationSliderChanged()
	{
        m_colorSaturation = m_colorSaturationSlider.value;
	}

    public void OnColorBrightnessSliderChanged()
	{
        m_colorBrightness = m_colorBrightnessSlider.value;
	}

    public void OnColorAlphaSliderChanged()
	{
        m_colorAlpha = m_colorAlphaSlider.value;
	}

    public void OnColorHueSpeedSliderChanged()
	{
        m_colorHueSpeed = m_colorHueSpeedSlider.value;
	}

    public void OnRotationOffsetXSliderChanged()
	{
        m_rotationX = m_rotationOffsetXSlider.value;
	}        

    public void OnRotationOffsetYSliderChanged()
	{
        m_rotationY = m_rotationOffsetYSlider.value;
	}        

    public void OnRotationOffsetZSliderChanged()
	{
        m_rotationZ = m_rotationOffsetZSlider.value;
	}        
    public void OnTubeLengthSliderChanged()
	{
        m_tubeSpacing = m_tubeLengthSlider.value;
	}
#endregion

#region Toggle Functions
    public void OnRotateXToggleChanged()
	{
        m_rotateX = m_rotateXToggle.isOn;
	}

    public void OnRotateYToggleChanged()
	{
        m_rotateY = m_rotateYToggle.isOn;
	}

    public void OnRotateZToggleChanged()
	{
        m_rotateZ = m_rotateZToggle.isOn;
	}

    public void On3dModeToggleChanged()
	{
        m_3dMode = m_3dModeToggle.isOn;

        for(int i = 0; i < m_objects.Length; i++)
        {
            m_objects[i].GetComponent<Renderer>().sortingOrder = m_3dMode ? 0 : (m_objects.Length - 1 - i);
        }
	}
#endregion

}

