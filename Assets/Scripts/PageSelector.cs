
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class PageSelector : UdonSharpBehaviour
{
    [SerializeField] protected GameObject[] m_pages;
    //[SerializeField] protected GameObject[] m_pageButtons;
    //[SerializeField] protected Color m_selectedPageColor = Color.white;
    //[SerializeField] protected Color m_unselectedPageColor = new Color(1f, 1f, 1f, 0.5f);
    protected int m_pageIndex = 0;

	private void Start()
	{
        ShowPage1();
	}

    public void PageLeft()
	{
        ShowPage((m_pageIndex - 1 + m_pages.Length) % m_pages.Length);
	}

    public void PageRight()
	{
        ShowPage((m_pageIndex + 1 + m_pages.Length) % m_pages.Length);
	}

	public void ShowPage1()
    {
        ShowPage(0);
    }

    public void ShowPage2()
    {
        ShowPage(1);
    }

    public void ShowPage(int _pageIndex)
	{
        m_pageIndex = _pageIndex;
        for(int i = 0; i < m_pages.Length; i++)
		{
            bool showThisPage = _pageIndex == i;
            m_pages[i].SetActive(showThisPage);
		}
	}
}
