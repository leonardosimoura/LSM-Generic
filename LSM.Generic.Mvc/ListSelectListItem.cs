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
    [System.AttributeUsage(System.AttributeTargets.Property,AllowMultiple = false)]
    public class LabelForDropDown : System.Attribute
    {
        public LabelForDropDown()
        {
            
        }
    }

    public class ListSelectListItem
    {
        public static List<SelectListItem> GetLista<T>(dynamic Lista, object Id = null, string Placeholder = "") where T : class
        {
            try
            {
                if (Lista is IEnumerable<T>)
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

                        foreach (var item in Lista)
                        {
                            items.Add(new SelectListItem()
                            {
                                Value = propKey.GetValue(item, null).ToString(),
                                Text = propLabel.GetValue(item, null).ToString()
                            });
                        }

                        try
                        {
                            items.Find(p => p.Value == Id.ToString()).Selected = true;
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
