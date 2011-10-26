using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace LoggingServer.Interface.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Outputs a checkbox with a value other than true/false
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxWithValue(this HtmlHelper htmlHelper, string name, object value)
        {
            return CheckBoxWithValue(htmlHelper, name, false, value, null);
        }

        /// <summary>
        /// Outputs a checkbox with a value other than true/false
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="isChecked"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxWithValue(this HtmlHelper htmlHelper, string name, bool isChecked, object value)
        {
            return CheckBoxWithValue(htmlHelper, name, isChecked, value, null);
        }

        /// <summary>
        /// Outputs a checkbox with a value other than true/false
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="isChecked"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxWithValue(this HtmlHelper htmlHelper, string name, bool isChecked, object value, object htmlAttributes)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            var tagBuilder = CheckboxTagBuilder(name, isChecked, value, new RouteValueDictionary(htmlAttributes));
            return htmlHelper.Raw(tagBuilder);
        }

        /// <summary>
        /// Outputs a group of checkboxes, one for each enum value and a hidden field at the end to hold the transformed value.
        /// The checkbox names are prefixed with "Not." in order for the formUtilities.js script to work with the flag enum form posts.
        /// A special html attribute is also added to the checkboxes to signify the flag enum type.
        /// The hidden field is what is used to hold the actual value to bind.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxesForFlagsEnum<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value)
        {
            return CheckBoxesForFlagsEnum(htmlHelper, expression, value, null);
        }

        /// <summary>
        /// Outputs a group of checkboxes, one for each enum value and a hidden field at the end to hold the transformed value.
        /// The checkbox names are prefixed with "Not." in order for the formUtilities.js script to work with the flag enum form posts.
        /// A special html attribute is also added to the checkboxes to signify the flag enum type.
        /// The hidden field is what is used to hold the actual value to bind.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="value"></param>
        /// <param name="checkBoxHtmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxesForFlagsEnum<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value, object checkBoxHtmlAttributes)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            return CheckBoxesForFlagsEnum<TProperty>(htmlHelper, name, value, checkBoxHtmlAttributes);
        }

        /// <summary>
        /// Outputs a group of checkboxes, one for each enum value and a hidden field at the end to hold the transformed value.
        /// The checkbox names are prefixed with "Not." in order for the formUtilities.js script to work with the flag enum form posts.
        /// A special html attribute is also added to the checkboxes to signify the flag enum type.
        /// The hidden field is what is used to hold the actual value to bind.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="checkBoxHtmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxesForFlagsEnum<T>(this HtmlHelper htmlHelper, string name, object value, object checkBoxHtmlAttributes)
        {
            if ((!typeof(T).IsEnum && !IsNullableEnum<T>()) || !HasFlagsAttribute<T>())
                throw new ArgumentException("TProperty must be a flags enum");
            var checkBoxes = new StringBuilder();
            var checkedValue = value ?? GetDefaultValue<T>();
            var enumValues = Enum.GetValues(checkedValue.GetType());
            foreach (var enumValue in enumValues)
            {
                var isChecked = value != null && checkedValue.ToString().Split(',').Any(x => x.Trim() == enumValue.ToString());
                var attributes = new RouteValueDictionary(checkBoxHtmlAttributes) { { "flaggedenum", "true" } };
                var notName = string.Format("Not.{0}", name);
                var checkboxTagBuilder = CheckboxTagBuilder(notName, isChecked, enumValue, attributes);
                var label = LabelTagBuilder(notName, enumValue.ToString());
                checkBoxes.AppendFormat("{0} {1}\r\n", label, checkboxTagBuilder);
            }
            var hiddenTagBuilder = HiddenTagBuilder(value, name);
            checkBoxes.Append(hiddenTagBuilder);
            return htmlHelper.Raw(checkBoxes.ToString());
        }

        private static string LabelTagBuilder(string labelFor, string label)
        {
            var labelTagBuilder = new TagBuilder("label");
            labelTagBuilder.MergeAttribute("for", labelFor.Replace('.', '_'));
            labelTagBuilder.InnerHtml = label;
            return labelTagBuilder.ToString(TagRenderMode.Normal);
        }

        /// <summary>
        /// Safely routes a link to a specific area.
        /// </summary>
        public static IHtmlString ActionLinkArea<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string linkText, string area) where TController : Controller
        {
            return ActionLinkArea(htmlHelper, action, linkText, area, null);
        }

        /// <summary>
        /// Safely routes a link to a specific area.
        /// </summary>
        public static IHtmlString ActionLinkArea<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string linkText, string area, object htmlAttributes) where TController : Controller
        {
            var routingValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);
            routingValues.Add("area", area);
            return htmlHelper.RouteLink(linkText, routingValues, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        private static string HiddenTagBuilder(object value, string name)
        {
            var hiddenTagBuilder = new TagBuilder("input");
            hiddenTagBuilder.MergeAttribute("type", "hidden");
            hiddenTagBuilder.MergeAttribute("name", name, true);
            hiddenTagBuilder.MergeAttribute("value", value != null ? value.ToString() : string.Empty, true);
            hiddenTagBuilder.GenerateId(name);
            return hiddenTagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static string CheckboxTagBuilder(string name, bool isChecked, object value, RouteValueDictionary htmlAttributes)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", name, true);
            if (isChecked)
                tagBuilder.MergeAttribute("checked", "checked");
            tagBuilder.MergeAttribute("value", value.ToString(), true);
            tagBuilder.GenerateId(name);
            return tagBuilder.ToString(TagRenderMode.SelfClosing);
        }

        private static bool HasFlagsAttribute<T>()
        {
            var flags = typeof(T).GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            var nullableFlags = underlyingType != null && underlyingType.GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
            return flags || nullableFlags;
        }

        private static bool IsNullableEnum<T>()
        {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            return underlyingType != null && underlyingType.IsEnum;
        }

        private static T GetDefaultValue<T>()
        {
            var underlyingType = Nullable.GetUnderlyingType(typeof(T));
            if (underlyingType != null)
                return (T)Activator.CreateInstance(underlyingType);
            return default(T);
        }
    }
}