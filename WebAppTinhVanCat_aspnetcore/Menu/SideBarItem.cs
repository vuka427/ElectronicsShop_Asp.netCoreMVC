using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;

namespace WebAppTinhVanCat_aspnetcore.Menu
{

    public enum SidebarItemType
    {
        Divider,
        Heading,
        NavItem
    }


    public class SideBarItem
    {
        public string Title { get; set; } // tiêu đề
        public bool IsActive { get; set; } // đanh dấu thể có class "active"
        public SidebarItemType Type { get; set; } // kiểu hiện thị:  Divider (gạch ngang ), Heading (tiêu đề),  NavItem (menu link)
        public string Area { get; set; } 
        public string Controller { get; set; }
        public string Action { get; set; }
        public string AwesomeIcon { get; set; } // icon 


        public List<SideBarItem> Items { get; set; } // danh sách thẻ con cho kiểu  NavItem
        public string CollapseId { get; set; } 


        public string GetLink(IUrlHelper urlHelper) // phát sinh url dựa theo Action,Controller, Area
        {
            return  urlHelper.Action(Action,Controller, new {area = Area });
            
        }
        
        
        public string RenderHtml(IUrlHelper urlHelper) // phát sinh thẻ Html
        {
            var Html = new StringBuilder();

            if (Type == SidebarItemType.Divider)
            {
                Html.Append("<hr class=\"sidebar-divider\"/>");
            }
            else if (Type == SidebarItemType.Heading)
            {
                Html.Append($"<div class=\"sidebar-heading\">\r\n {Title} \r\n</div>\r\n");
            }
            else if (Type == SidebarItemType.NavItem)
            {
                if (Items == null)
                {
                    var url = GetLink(urlHelper);
                    var icon = (AwesomeIcon != null) ? $" <i class=\"{AwesomeIcon}\"></i>" : "";
                    var cssClass = "nav-item";
                    if (IsActive) cssClass += " active";
                    Html.Append(@$"
                        <li class=""{cssClass}"">
                            <a class=""nav-link"" href=""{url}"">
                               {icon}
                                <span>{Title}</span>
                            </a>
                        </li>
                    ");
                }
                else
                {

                    var icon = (AwesomeIcon != null) ? $" <i class=\"{AwesomeIcon}\"></i>" : "";

                    var cssClass = "nav-item";
                    if (IsActive) cssClass += " active";

                    var cssCollaspse = "collapse";
                    if (IsActive) cssCollaspse += " show";

                    var itemMenu = "";
                    foreach (var item in Items)
                    {
                        var urlItem = item.GetLink(urlHelper);
                        var cssItem = "collapse-item";
                        if (item.IsActive) cssItem += " active";
                        itemMenu += $" <a class=\"{cssItem}\" href=\"{urlItem}\">{item.Title}</a>";
                    }

                    Html.Append($@"
                        <li class=""{cssClass}"">
                            <a class=""nav-link collapsed"" href=""#"" data-toggle=""collapse"" data-target=""#{CollapseId}""
                               aria-expanded=""true"" aria-controls=""{CollapseId}"">
                                {icon}
                                <span>{Title}</span>
                            </a>
                            <div id=""{CollapseId}"" class=""{cssCollaspse}"" aria-labelledby=""headingTwo"" data-parent=""#accordionSidebar"">
                                <div class=""bg-white py-2 collapse-inner rounded"">
                                    {itemMenu}
                                </div>
                            </div>
                        </li>
                    ");
                }



            }
            
            



            return Html.ToString();
        }

    }
}
