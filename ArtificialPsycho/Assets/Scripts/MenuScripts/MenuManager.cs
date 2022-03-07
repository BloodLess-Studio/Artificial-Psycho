using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    /*-------- Inspector --------*/
    [Header("Menu List")]
    [SerializeField] private Menu[] menus;

    /*-------- Static Variables --------*/
    public static MenuManager Instance;

    /*-------- Private Variables --------*/
    private Menu currentMenu;

    /*-------- Unity Starting Events --------*/
    #region
    private void Awake()
    {
        Instance = this;
    }
    #endregion
    /*-------- Public Methods --------*/
    #region
    public void ChangeMenu(string menuName)
    {
        foreach (Menu menu in menus)
        {
            if (menu.Name == menuName)
            {
                menu.Open();

                if (currentMenu != null)
                    currentMenu.Close();
                currentMenu = menu;
            }
                
        }
    }

    public void ChangeMenu(Menu menu)
    {
        menu.Open();

        if (currentMenu != null)
            currentMenu.Close();
        currentMenu = menu;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
