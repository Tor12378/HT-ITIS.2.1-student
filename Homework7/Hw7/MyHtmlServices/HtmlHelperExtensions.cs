using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.RegularExpressions;
namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel<TModel>(this IHtmlHelper<TModel?> helper)
    {
        var modelType = helper.ViewData.ModelExplorer.ModelType;
        var model = helper.ViewData.Model;
        var htmlContent = new HtmlContentBuilder();

        foreach (var property in modelType.GetProperties())
        {
            htmlContent.AppendHtml(CreatePropertyEditor(property, model!));
        }

        return htmlContent;
    }

    private static IHtmlContent CreatePropertyEditor(PropertyInfo propertyInfo, object? model)
    {
        var htmlContent = new TagBuilder("div");
        htmlContent.InnerHtml.AppendHtml(CreateLabel(propertyInfo));
        if (!propertyInfo.PropertyType.IsEnum)
            htmlContent.InnerHtml.AppendHtml(CreateInput(propertyInfo));
        else
            htmlContent.InnerHtml.AppendHtml(CreateSelect(propertyInfo));
        
        htmlContent.InnerHtml.AppendHtml(GenerateValidationMessage(propertyInfo, model));
        return htmlContent;
    }

    private static IHtmlContent GenerateValidationMessage(PropertyInfo propertyInfo, object? model)
    {
        var validationMessage = new TagBuilder("span");
        validationMessage.InnerHtml.AppendHtml(string.Empty);
        if (model == null)
            return validationMessage;

        var validationAttributes = propertyInfo.GetCustomAttributes(typeof(ValidationAttribute), true);

        foreach (ValidationAttribute validationAttribute in validationAttributes)
        {
            if (validationAttribute.IsValid(propertyInfo.GetValue(model))) continue;
            validationMessage.InnerHtml.AppendHtml(validationAttribute.ErrorMessage!);
            return validationMessage;
        }

        return validationMessage;
    }

    private static IHtmlContent CreateSelect(PropertyInfo propertyInfo)
    {
        var select = new TagBuilder("select");
        var enumValues = propertyInfo.PropertyType.GetEnumValues();
        select.Attributes.Add("id", propertyInfo.Name);

        foreach (var value in enumValues)
        {
            select.InnerHtml.AppendHtml($"<option value=\"{value}\">{value}</option>");
        }

        return select;
    }

    private static IHtmlContent CreateInput(PropertyInfo propertyInfo)
    {
        var input = new TagBuilder("input");
        var inputType = propertyInfo.PropertyType == typeof(string) ? "text" : "number";
        input.Attributes.Add("type", inputType);
        input.Attributes.Add("id", propertyInfo.Name);
        input.Attributes.Add("name", propertyInfo.Name);
        return input;
    }

    private static IHtmlContent CreateLabel(PropertyInfo propertyInfo)
    {
        var label = new TagBuilder("label");
        var displayAttribute = propertyInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
        var labelName = displayAttribute?.Name ?? SeparateName(propertyInfo.Name);
        label.InnerHtml.AppendHtml(labelName);
        label.Attributes.Add("for", propertyInfo.Name);
        return label;
    }

    private static string SeparateName(string name) =>
        Regex.Replace(name, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled);
}
