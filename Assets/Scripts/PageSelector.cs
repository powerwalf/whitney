using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PageSelector : UdonSharpBehaviour
{
    [SerializeField] protected GameObject[] m_pages;

    [UdonSynced, FieldChangeCallback(nameof(PageIndex))] 
    protected int m_pageIndex = 0;
    protected int PageIndex
    {
        set
        {
            BecomeOwnerIfNotAlready();
            m_pageIndex = value;
            RequestSerialization();
            UpdatePageVisual();
        }
    }
    
	private void Start()
	{
        ShowPage(0);
	}

    public void PageLeft()
	{
        ShowPage((m_pageIndex - 1 + m_pages.Length) % m_pages.Length);
	}

    public void PageRight()
	{
        ShowPage((m_pageIndex + 1 + m_pages.Length) % m_pages.Length);
	}

    public void ShowPage(int _pageIndex)
	{
        //BecomeOwnerIfNotAlready();
        PageIndex = _pageIndex;
	}

	public void UpdatePageVisual()
	{
        for(int i = 0; i < m_pages.Length; i++)
		{
            bool showThisPage = m_pageIndex == i;
            m_pages[i].SetActive(showThisPage);
		}
	}

	public override void OnDeserialization()
	{
        UpdatePageVisual();
	}

	public void BecomeOwnerIfNotAlready()
    {
        if(!Networking.IsOwner(Networking.LocalPlayer, this.gameObject))
		{
            Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
		}
    }
}

