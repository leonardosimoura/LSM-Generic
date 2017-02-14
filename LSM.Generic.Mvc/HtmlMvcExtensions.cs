using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Mvc.Html
{
    public static class HtmlMvcExtensions
    {
        public static HtmlTable<TModel> TableFor<TModel>(this HtmlHelper html, IEnumerable<TModel> model)
        {
            return new HtmlTable<TModel>(html, model);
        }

        private class HtmlCommand
        {
            public bool Show { get; set; } = true;
            public string Label { get; set; }
            public string Action { get; set; }
            public string Controller { get; set; }
            public string CustomClass { get; set; }
            public string BtnClass { get; set; }
            public string GlyphIcon { get; set; }
            public string FontAwesome { get; set; }

            private string htmlIcon = string.Empty;
            private string url = string.Empty;
            public string ToHtmlString(TableIconStyle iconStyle, UrlHelper urlHelper)
            {

                switch (iconStyle)
                {
                    case TableIconStyle.Glyphicons:
                        htmlIcon = "<span class='" + ((!String.IsNullOrEmpty(CustomClass)) ? CustomClass : "glyphicon " + GlyphIcon)
                                        + "' aria-hidden='true'></span>";
                        break;
                    case TableIconStyle.FontAwesome:
                        htmlIcon = "<i class='" + ((!String.IsNullOrEmpty(CustomClass)) ? CustomClass : "fa " + FontAwesome)
                                        + "' aria-hidden='true'></i>";
                        break;
                    default:
                        break;
                }
                                

                if (Action.EndsWith(")") || Action.EndsWith(");"))
                {
                    url = Action;

                    return "<a style='margin-right:8px;' class='btn " + BtnClass + "' "
                            + " href='javascript:void(0);' onclick='"
                            + url + "' title='" + Label + "' > "
                            + htmlIcon + " </a>";
                }
                else
                {
                    if (String.IsNullOrEmpty(Controller))
                        url = urlHelper.Action(Action);
                    else
                        url = urlHelper.Action(Action, Controller);

                    return "<a style='margin-right:8px;' class='btn " + BtnClass + "' href='" + url + "' title='" + Label + "'> "
                          + htmlIcon + " </a>";
                }
            }
        }

        public class HtmlTable<TModel> : IHtmlString
        {
            private IEnumerable<TModel> _items;
            private HtmlHelper _html;
            private List<string> _props = new List<string>();
            private HtmlCommand _edit = new HtmlCommand()
            {
                BtnClass = "btn-warning",
                GlyphIcon = "glyphicon-pencil",
                FontAwesome = "fa-pencil-square-o",
                Action = "Edit"
            };
            private HtmlCommand _delete = new HtmlCommand()
            {
                BtnClass = "btn-danger",
                GlyphIcon = "glyphicon glyphicon-trash",
                FontAwesome = "fa-trash-o",
                Action = "Delete"
            };
            private HtmlCommand _detail = new HtmlCommand()
            {
                BtnClass = "btn-info",
                GlyphIcon = "glyphicon glyphicon-search",
                FontAwesome = "fa-search",
                Action = "Detail"
            };
            private List<HtmlCommand> _listCommands = new List<HtmlCommand>();
            private TableIconStyle _iconStyle = TableIconStyle.Glyphicons;

            public HtmlTable(HtmlHelper html, IEnumerable<TModel> model)
            {
                _html = html;
                _items = model;
            }

            public HtmlTable<TModel> IconStyle(TableIconStyle iconStyle)
            {
                _iconStyle = iconStyle;
                return this;
            }

            #region CustomComands

            public HtmlTable<TModel> Command(string label)
            {
                this.PrivateCommand(label, null, null, null, null);
                return this;
            }

            public HtmlTable<TModel> Command(string label, string action)
            {
                this.PrivateCommand(label, action, null, null, null);
                return this;
            }

            public HtmlTable<TModel> Command(string label, string action, string controller)
            {
                this.PrivateCommand(label, action, controller, null, null);
                return this;
            }

            public HtmlTable<TModel> Command(string label, string action, string controller, string customBtnClass, string customIconClass)
            {
                this.PrivateCommand(label, action, controller, customBtnClass, customIconClass);
                return this;
            }

            private void PrivateCommand(string label, string action, string controller, string customBtnClass, string customClass)
            {
                var cmd = new HtmlCommand();
                cmd.BtnClass = customBtnClass;
                cmd.Show = true;
                cmd.Label = label;
                cmd.Action = action;
                cmd.Controller = controller;
                cmd.BtnClass = customBtnClass;
                cmd.CustomClass = customClass;
                _listCommands.Add(cmd);
            }

            #endregion

            public HtmlTable<TModel> ForMember<TValue>(Expression<Func<TModel, TValue>> expressao)
            {
                var expression = (MemberExpression)expressao.Body;
                string propriedade = expression.Member.Name;
                _props.Add(propriedade);
                return this;
            }


            public HtmlTable<TModel> ShowEdit(bool show)
            {
                _edit.Show = show;
                return this;
            }
            public HtmlTable<TModel> Edit(string label)
            {
                this.PrivateEdit(label, null, null, null);
                return this;
            }

            public HtmlTable<TModel> Edit(string label, string action)
            {
                this.PrivateEdit(label, action, null, null);
                return this;
            }

            public HtmlTable<TModel> Edit(string label, string action, string controller)
            {
                this.PrivateEdit(label, action, controller, null);
                return this;
            }

            public HtmlTable<TModel> Edit(string label, string action, string controller, string customCss)
            {
                this.PrivateEdit(label, action, controller, customCss);
                return this;
            }

            private void PrivateEdit(string label, string action, string controller, string customClass)
            {
                _edit.Label = label;
                _edit.Action = action;
                _edit.Controller = controller;
                _edit.CustomClass = customClass;
            }

            public HtmlTable<TModel> ShowDelete(bool show)
            {
                _delete.Show = show;
                return this;
            }

            public HtmlTable<TModel> Delete(string label)
            {
                PrivateDelete(label);
                return this;
            }

            public HtmlTable<TModel> Delete(string label, string action)
            {
                PrivateDelete(label, action);
                return this;
            }

            public HtmlTable<TModel> Delete(string label, string action, string controller)
            {
                PrivateDelete(label, action, controller);
                return this;
            }

            public HtmlTable<TModel> Delete(string label, string action, string controller, string customClass)
            {
                PrivateDelete(label, action, controller, customClass);
                return this;
            }

            private void PrivateDelete(string label, string action = "", string controller = "", string customClass = "")
            {
                _delete.Label = label;
                _delete.Action = action;
                _delete.Controller = controller;
                _delete.CustomClass = customClass;
            }

            public HtmlTable<TModel> ShowDetail(bool show)
            {
                _detail.Show = show;
                return this;
            }

            public HtmlTable<TModel> Detail(string label)
            {
                PrivateDetail(label);
                return this;
            }
            public HtmlTable<TModel> Detail(string label, string action)
            {
                PrivateDetail(label, action);
                return this;
            }

            public HtmlTable<TModel> Detail(string label, string action, string controller)
            {
                PrivateDetail(label, action, controller);
                return this;
            }

            public HtmlTable<TModel> Detail(string label, string action, string controller, string customClass)
            {
                PrivateDetail(label, action, controller, customClass);
                return this;
            }

            private void PrivateDetail(string label, string action = "", string controller = "", string customClass = "")
            {
                _detail.Label = label;
                _detail.Action = action;
                _detail.Controller = controller;
                _detail.CustomClass = customClass;
            }

            public string ToHtmlString()
            {
                var type = typeof(TModel);

                if (_items == null)
                {
                    _items = new List<TModel>();
                }

                var table = new StringBuilder();

                table.Append(@"<table class='table table-bordered table-hovered'>");
                table.Append("<tr>");

                foreach (var item in _props)
                {
                    foreach (var prop in type.GetProperties().Where(w => w.Name == item))
                    {
                        if (prop.GetCustomAttributes(typeof(DisplayAttribute), false).Any())
                        {
                            var attr = prop.GetCustomAttributes(typeof(DisplayAttribute), false).First() as DisplayAttribute;
                            table.Append("<th>" + attr.Name + " </th>");
                        }
                        else if (prop.GetCustomAttributes(typeof(DisplayNameAttribute), false).Any())
                        {
                            var attr = prop.GetCustomAttributes(typeof(DisplayNameAttribute), false).First() as DisplayNameAttribute;
                            table.Append("<th>" + attr.DisplayName + " </th>");
                        }
                        else
                        {
                            table.Append("<th>" + prop.Name + " </th>");
                        }
                    }
                }

                table.Append("<th> </th>");
                table.Append("<tr>");

                foreach (var item in _props)
                {
                    foreach (var model in _items)
                    {
                        table.Append("<tr>");
                        foreach (var prop in type.GetProperties().Where(w => _props.ToList().Contains(w.Name)))
                        {
                            if (prop.GetCustomAttributes(typeof(DisplayFormatAttribute), false).Any())
                            {
                                var attr = prop.GetCustomAttributes(typeof(DisplayFormatAttribute), false).First() as DisplayFormatAttribute;
                                table.Append("<td>" + String.Format(attr.DataFormatString, prop.GetValue(model)) + " </td>");
                            }
                            else
                            {
                                table.Append("<td>" + prop.GetValue(model).ToString() + " </td>");
                            }
                        }
                        var _urlHelper = new UrlHelper(_html.ViewContext.RequestContext);
                        if (_edit.Show || _detail.Show || _delete.Show)
                        {
                            table.Append("<td>");
                            if (_edit.Show)
                                table.Append(_edit.ToHtmlString(_iconStyle,  _urlHelper));

                            if (_detail.Show)
                                table.Append(_detail.ToHtmlString(_iconStyle,  _urlHelper));

                            if (_delete.Show)
                                table.Append(_delete.ToHtmlString(_iconStyle,  _urlHelper));

                            if (_listCommands.Any())
                            {
                                foreach (var cmd in _listCommands)
                                {
                                    table.Append(cmd.ToHtmlString(_iconStyle, _urlHelper));
                                }
                            }

                            table.Append("</td>");
                        }

                        

                        table.Append("</tr>");
                    }
                }

                table.Append("<tr>");
                table.Append("</table>");
                return table.ToString();
            }
        }
    }


    public enum TableIconStyle
    {
        Glyphicons = 0,
        FontAwesome = 1
    }

}
