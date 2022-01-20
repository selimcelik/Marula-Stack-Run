using UnityEngine;

namespace ManagerActorFramework.Utils
{
    public enum MenuType
    {
        Menu,
        SubMenu,
        Child
    }

    public abstract class MB_UiActor<TManager> : Actor<TManager> where TManager : Manager<TManager>
    {
        [HideInInspector] public GameObject SelectedMenu;
        [HideInInspector] public GameObject SelectedSubMenu;
        [HideInInspector] public GameObject SelectedChild;

        public void OpenPanel(GameObject panel, MenuType menuType)
        {
            if (menuType == MenuType.Menu)
            {
                HidePanel(MenuType.Child);
                HidePanel(MenuType.SubMenu);
                HidePanel(MenuType.Menu);

                panel.SetActive(true);
                SelectedMenu = panel;
            }
            else if (menuType == MenuType.SubMenu)
            {
                HidePanel(MenuType.Child);
                HidePanel(MenuType.SubMenu);

                panel.SetActive(true);
                SelectedSubMenu = panel;
            }
            else
            {
                HidePanel(MenuType.Child);

                panel.SetActive(true);
                SelectedChild = panel;
            }
        }

        public bool HidePanel(MenuType menuType)
        {
            switch (menuType)
            {
                case MenuType.Menu:
                    if (SelectedMenu != null)
                    {
                        SelectedMenu.SetActive(false);
                        SelectedMenu = null;
                        return true;
                    }
                    return false;

                case MenuType.SubMenu:
                    if (SelectedSubMenu != null)
                    {
                        SelectedSubMenu.SetActive(false);
                        SelectedSubMenu = null;
                        return true;
                    }
                    return false;

                case MenuType.Child:
                    if (SelectedChild != null)
                    {
                        SelectedChild.SetActive(false);
                        SelectedChild = null;
                        return true;
                    }
                    return false;

                default: return false;
            }
        }
    }
}