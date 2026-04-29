using Booking.Autos.API.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Booking.Autos.API.Filters
{
    public sealed class RequiredStringNoWhitespaceFilter : IAsyncActionFilter
    {
        private static readonly NullabilityInfoContext NullabilityContext = new();

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            if (!HttpMethods.IsPost(method) && !HttpMethods.IsPut(method))
                return next();

            var errors = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            var visited = new HashSet<object>(ReferenceEqualityComparer.Instance);

            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value is null)
                    continue;

                ValidateObject(argument.Value, argument.Key, errors, visited);
            }

            if (errors.Count > 0)
            {
                var response = new ApiErrorResponse(
                    "Errores de validación",
                    errors.ToDictionary(x => x.Key, x => x.Value.ToArray()));

                context.Result = new BadRequestObjectResult(response);
                return Task.CompletedTask;
            }

            return next();
        }

        private static void ValidateObject(
            object instance,
            string prefix,
            IDictionary<string, List<string>> errors,
            ISet<object> visited)
        {
            if (!visited.Add(instance))
                return;

            var type = instance.GetType();
            if (IsTerminalType(type))
                return;

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!property.CanRead || property.GetIndexParameters().Length > 0)
                    continue;

                var value = property.GetValue(instance);
                var propertyName = GetSerializedPropertyName(property);
                var fullPath = string.IsNullOrWhiteSpace(prefix)
                    ? propertyName
                    : $"{prefix}.{propertyName}";

                if (property.PropertyType == typeof(string))
                {
                    var nullability = NullabilityContext.Create(property);
                    var isRequired = nullability.ReadState == NullabilityState.NotNull;

                    if (isRequired && string.IsNullOrWhiteSpace(value as string))
                    {
                        AddError(errors, fullPath, $"El campo {propertyName} es obligatorio y no puede contener solo espacios.");
                    }

                    continue;
                }

                if (value is null)
                    continue;

                if (value is IEnumerable enumerable and not string)
                {
                    var index = 0;
                    foreach (var item in enumerable)
                    {
                        if (item is null)
                        {
                            index++;
                            continue;
                        }

                        ValidateObject(item, $"{fullPath}[{index}]", errors, visited);
                        index++;
                    }

                    continue;
                }

                ValidateObject(value, fullPath, errors, visited);
            }
        }

        private static bool IsTerminalType(Type type)
        {
            return type.IsPrimitive
                || type.IsEnum
                || type == typeof(string)
                || type == typeof(decimal)
                || type == typeof(DateTime)
                || type == typeof(DateTimeOffset)
                || type == typeof(TimeSpan)
                || type == typeof(Guid);
        }

        private static string GetSerializedPropertyName(PropertyInfo property)
        {
            return property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name
                ?? property.Name;
        }

        private static void AddError(
            IDictionary<string, List<string>> errors,
            string key,
            string message)
        {
            if (!errors.TryGetValue(key, out var list))
            {
                list = new List<string>();
                errors[key] = list;
            }

            list.Add(message);
        }
    }
}
