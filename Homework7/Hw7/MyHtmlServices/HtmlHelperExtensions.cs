using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hw7.MyHtmlServices;

public static class HtmlHelperExtensions
{
    public static IHtmlContent MyEditorForModel(this IHtmlHelper helper)
    {
        var model = helper.ViewData.Model;
        var properties = helper.ViewData.ModelMetadata.ModelType.GetProperties();
        var html = new HtmlContentBuilder();
        foreach (var property in properties)
        {
            var label = GenerateLabel(property);
            var input = GenerateInput(property, model);
            var span = GenerateSpan(property, model);
            var line = $"<div>{label}<br>{(property.PropertyType.IsEnum ? GenerateDropdown(property) : input)}{span}</div>";
            html.AppendHtmlLine(line);
        }
        return html;
    }

    private static string GenerateLabel(PropertyInfo property)
    {
        var display = property.GetCustomAttributes().OfType<DisplayAttribute>().FirstOrDefault();
        var name = display != null ? display.Name : SplitCamelCase(property.Name);
        return $"<label for=\"{property.Name}\">{name}</label>";
    }
    
    private static string GenerateInput(PropertyInfo property, object? model)
    {
        return $"<input class=\"text-box single-line\" " +
               $"id=\"{property.Name}\" " +
               $"name=\"{property.Name}\" " +
               $"type=\"{(property.PropertyType == typeof(string) ? "text" : "number")}\" " +
               $"value=\"{(model == null ? "" : property.GetValue(model))}\">";
    }
    
    private static string SplitCamelCase(string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
    }

    private static string GenerateSpan(PropertyInfo property, object? model)
    {
        if (model is not null)
        {
            var valAtrs = property.GetCustomAttributes().OfType<ValidationAttribute>();
            foreach (var atr in valAtrs)
            {
                if(!atr.IsValid(property.GetValue(model)))
                    return $"<span class=\"field-validation-valid\" data-valmsg-for=\"{property.Name}\" data-valmsg-replace=\"true\">{atr.ErrorMessage}</span>";
            }
        }
        return
            $"<span class=\"field-validation-valid\" data-valmsg-for=\"{property.Name}\" data-valmsg-replace=\"true\"></span>";
    }

    private static string GenerateDropdown(PropertyInfo property)
    {
        var sb = new StringBuilder();
        foreach (var val in property.PropertyType.GetEnumValues())
        {
            sb.Append($"<option value=\"{val}\">{val}</option>\n");
        }
        return $"<select>{sb.ToString()}</select>";
    }
}