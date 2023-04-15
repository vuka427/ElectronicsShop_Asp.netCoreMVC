using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppTinhVanCat_aspnetcore.Menu
{
    /// <summary>
    /// phát sinh tất cả các phần tử side bar cho admin dashboard
    /// </summary>
    public class AdminSideBarService
    {

        private readonly IUrlHelper UrlHelper;
        public List<SideBarItem> Items { get; set; } = new List<SideBarItem>();

        public AdminSideBarService(IUrlHelperFactory factory, IActionContextAccessor action )
        {
            UrlHelper =  factory.GetUrlHelper(action.ActionContext);
            // khai báo các phần tử cho side bar

            
            
            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
            Items.Add(new SideBarItem()//Product
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý sản phẩm",
                AwesomeIcon = "fa fa-store",
                CollapseId = "product",
                Items = new List<SideBarItem>()
                {
                    new SideBarItem() // các sản phẩm
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Product",
                            Action = "Index",
                            Area = "Product",
                            Title = "Danh sách sản phẩm"
                        },
                    new SideBarItem() // thêm sản phẩm
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Product",
                            Action = "Create",
                            Area = "Product",
                            Title = "Thêm sản phẩm"
                        },
                   new SideBarItem() // danh mục product
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "CategoryProduct",
                            Action = "Index",
                            Area = "Product",
                            Title = "Danh mục sản phẩm"
                        },
                    new SideBarItem() // Thương hiệu
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "TradeMark",
                            Action = "Index",
                            Area = "Product",
                            Title = "Thương hiệu "
                        }

                },
            });
            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
            Items.Add(new SideBarItem()//Order
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý đơn hàng",
                AwesomeIcon = "fa fa-clipboard-list",
                CollapseId = "order",
                Items = new List<SideBarItem>()
                {
                   new SideBarItem() // Hóa đơn
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "OrderManage",
                            Action = "Index",
                            Area = "Product",
                            Title = "Đơn hàng"
                        }
                },
            });
            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
            Items.Add(new SideBarItem()//blog
            {
                Type = SidebarItemType.NavItem,
                Title = "Quản lý bài viết",
                AwesomeIcon = "fa fa-pen-nib",
                CollapseId = "Blog-post",
                Items = new List<SideBarItem>()
                {
                   new SideBarItem() // danh mục blog
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Category",
                            Action = "Index",
                            Area = "Blog",
                            Title = "Chuyên mục Blog"
                        },
                    new SideBarItem() // thêm danh mục
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Category",
                            Action = "Create",
                            Area = "Blog",
                            Title = "Thêm chuyên mục"
                        },
                    new SideBarItem() // các bài viết
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Post",
                            Action = "Index",
                            Area = "Blog",
                            Title = "Các bài viết"
                        },
                    new SideBarItem() // thêm bài viết
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Post",
                            Action = "Create",
                            Area = "Blog",
                            Title = "Thêm bài viết"
                        }
                },
            });
            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
            Items.Add(new SideBarItem()
            { //Thống kê
                Type = SidebarItemType.NavItem,
                Controller = "Statistical",
                Action = "Index",
                Area = "Statistic",
                Title = "Thống kê",
                AwesomeIcon = "fas fa-chart-bar"

            });
            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
            Items.Add(new SideBarItem()
            { //Quản lý file
                Type = SidebarItemType.NavItem,
                Controller = "FileManager",
                Action = "Index",
                Area = "Files",
                Title = "Quản lý File",
                AwesomeIcon = "fa fa-file"

            });
            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
            Items.Add(new SideBarItem()//Phân quyền và thành viên
            {
                Type = SidebarItemType.NavItem,
                Title = "Phân quyền và User",
                AwesomeIcon = "fa fa-folder",
                CollapseId = "role",
                Items = new List<SideBarItem>()
                {
                   new SideBarItem() // Quản lý quyền
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Role",
                            Action = "Index",
                            Area = "Identity",
                            Title = "Các vai trò (Role)"
                        },
                    new SideBarItem() // thêm role
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "Role",
                            Action = "Create",
                            Area = "Identity",
                            Title = "Thêm Role mới"
                        },
                    new SideBarItem() // thêm role
                        {
                            Type = SidebarItemType.NavItem,
                            Controller = "User",
                            Action = "Index",
                            Area = "Identity",
                            Title = "Danh sách user"
                        }
                },
            });
            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
            Items.Add(new SideBarItem() { Type = SidebarItemType.Heading, Title = "Quản lý chung" });// Heading

            Items.Add(new SideBarItem()
            { //Quản lý Database
                Type = SidebarItemType.NavItem,
                Controller = "DbManage",
                Action = "Index",
                Area = "Database",
                Title = "Quản lý Database",
                AwesomeIcon = "fa fa-database"

            });

            Items.Add(new SideBarItem() // Quản lý liên hệ
            {
                Type = SidebarItemType.NavItem,
                Controller = "Contact",
                Action = "Index",
                Area = "Contact",
                Title = "Quản lý liên hệ",
                AwesomeIcon = "fa fa-address-book"

            });
            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
            Items.Add(new SideBarItem()
            { //Cấu hình Shop
                Type = SidebarItemType.NavItem,
                Title = "Cấu hình Shop",
                AwesomeIcon = "fa fa-window-restore",
                CollapseId = "config",
                Items = new List<SideBarItem>()
                    {
                          new SideBarItem() // Đơn vị tính
                              {
                                    Type = SidebarItemType.NavItem,
                                    Controller = "ConfigShop",
                                    Action = "UnitProduct",
                                    Area = "Product",
                                    Title = "Đơn vị tính"
                              }

                    }

            });

            Items.Add(new SideBarItem() { Type = SidebarItemType.Divider });// gạch ngang
          



        }

        public string renderHtml() 
        {
            var html = new StringBuilder();

            foreach (var item in Items)
            {
                html.Append(item.RenderHtml(UrlHelper)) ;
            }


            return html.ToString();
        }

        public void SetActive(string Controller,string Action,string Area)
        {
            foreach (var item in Items)
            {
                if (item.Controller == Controller && item.Action == Action && item.Area == Area)
                {
                    item.IsActive = true;
                    return;
                }
                else
                {
                    if (item.Items != null)
                    {
                        foreach (var childitem in item.Items)
                        {
                            if (childitem.Controller == Controller && childitem.Action == Action && childitem.Area == Area)
                            {
                                childitem.IsActive = true;
                                item.IsActive = true;
                                return;
                            }
                        }   
                    }
                }
            }
        }

    }
}
