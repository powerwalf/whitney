using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using UnityEngine.UI;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class SliderWithLabel : UdonSharpBehaviour
{
    [SerializeField] protected string m_headerString;
    [Tooltip("Only used on non whole number sliders.")]
    [SerializeField] protected string m_valueFormattingString = "0.00";
    [SerializeField] protected Text m_text;
    [SerializeField] protected Slider m_slider;

    [UdonSynced, FieldChangeCallback(nameof(SyncedSliderValue))] 
    [SerializeField] [Range(0f, 360f)]
    protected float m_syncedSliderValue = 0.0f;
    protected float SyncedSliderValue
    {
        set
        {
            m_syncedSliderValue = value;
            RequestSerialization();
        }
    }

	// called by UI.Slider.OnValueChanged 
	public void OnSliderValueChanged()
	{
        BecomeOwnerIfNotAlready();
        SyncedSliderValue = m_slider.value;
        UpdateSliderVisuals();
	}

    public void UpdateSliderVisuals()
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

	public override void OnDeserialization()
	{
		m_slider.SetValueWithoutNotify(m_syncedSliderValue);
		UpdateSliderVisuals();
	}

	public void BecomeOwnerIfNotAlready()
    {
        if(!Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
		{
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
		}
    }
}

