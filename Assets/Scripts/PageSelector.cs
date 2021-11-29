
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

	private void Start()
	{
        ShowPage1();
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
        for(int i = 0; i < m_pages.Length; i++)
		{
            bool showThisPage = _pageIndex == i;
            m_pages[i].SetActive(showThisPage);
		}
	}
}
