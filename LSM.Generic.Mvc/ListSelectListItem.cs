using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace LSM.Generic.Mvc
{
    /// <summary>
    /// For correct execution of LSM.Generic.Mvc.MvcMapper need add this in a property
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property,AllowMultiple = false)]
    public class LabelForDropDown : System.Attribute
    {
        public LabelForDropDown()
        {
            
        }
    }

    public class MvcMapper
    {

        /// <summary>
        /// Get a List<SelectListItem>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List">List of object</param>
        /// <param name="SelectedId">Id of the selected item</param>
        /// <param name="Placeholder">If u want a first option(Placeholder) for the List</param>
        /// <returns></returns>
        public static List<SelectListItem> GetListOfSelectListItem<T>(dynamic List, object SelectedId = null, string Placeholder = "") where T : class
        {
            try
            {
                if (List is IEnumerable<T> || List is List<T>)
                {
                    List<SelectListItem> items = new List<SelectListItem>();
                    Type t = typeof(T);

                    PropertyInfo[] pi = t.GetProperties();
                    PropertyInfo propKey = null;
                    PropertyInfo propLabel = null;

                    if (Placeholder != "")
                    {
                        items.Add(new SelectListItem() { Value = "", Text = Placeholder });
                    }

                    foreach (PropertyInfo item in pi)
                    {
                        try
                        {
                            var key = System.Attribute.GetCustomAttribute(item, typeof(KeyAttribute));

                            if (key != null)
                            {
                                propKey = item;
                            }
                        }
                        catch (Exception)
                        {

                        }
                        try
                        {
                            var label = System.Attribute.GetCustomAttribute(item, typeof(LabelForDropDown));
                            if (label != null)
                            {
                                propLabel = item;
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }

                    if (propKey != null)
                    {
                        if (propLabel == null)
                        {
                            propLabel = propKey;
                        }

                        foreach (var item in List)
                        {
                            items.Add(new SelectListItem()
                            {
                                Value = propKey.GetValue(item, null).ToString(),
                                Text = propLabel.GetValue(item, null).ToString()
                            });
                        }

                        try
                        {
                            items.Find(p => p.Value == SelectedId.ToString()).Selected = true;
                        }
                        catch (Exception)
                        {

                        }
                        return items;
                    }
                    else
                    {
                        if (CultureInfo.CurrentCulture.ToString() == "pt-BR")
                        {
                            throw new Exception("Não existe um atributo Key nessa classe!");
                        }
                        else
                        {
                            throw new Exception("There is no Key attribute on this class!");
                        }                        
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
