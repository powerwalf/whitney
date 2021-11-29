using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class SliderWithLabel : UdonSharpBehaviour
{
    [SerializeField] protected string m_headerString;
    [Tooltip("Only used on non whole number sliders.")]
    [SerializeField] protected string m_valueFormattingString = "0.00";
    [SerializeField] protected Text m_text;
    [SerializeField] protected Slider m_slider;


    void Start()
    {
        OnSliderValueChanged();
    }

    public void OnSliderValueChanged()
	{
        if(m_slider.wholeNumbers)
		{
            m_text.text = m_headerString + " " + Mathf.RoundToInt(m_slider.value);
		}
		else
		{
            m_text.text = m_headerString + " " + m_slider.value.ToString(m_valueFormattingString);
		}
	}        
}

