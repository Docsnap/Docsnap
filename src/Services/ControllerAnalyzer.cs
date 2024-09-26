using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Docsnap.Services
{
    public class ControllerInfo
    {
        public string Name { get; set; }
        public List<string> Actions { get; set; }
    }

    public class ControllerAnalyzer
    {
        public List<ControllerInfo> AnalyzeControllers(string assemblyPath)
        {
            var controllers = new List<ControllerInfo>();

            try
            {
                var assembly = Assembly.LoadFrom(assemblyPath);
                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    if (IsController(type))
                    {
                        var controllerInfo = new ControllerInfo
                        {
                            Name = type.Name,
                            Actions = new List<ActionInfo>()
                        };

                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                        foreach (var method in methods)
                        {
                            var actionInfo = GetActionInfo(method);
                            if (actionInfo != null)
                            {
                                controllerInfo.Actions.Add(actionInfo);
                            }
                        }

                        controllers.Add(controllerInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao analisar o assembly: {ex.Message}");
            }

            return controllers;
        }

        private bool IsController(Type type)
        {
            return typeof(Microsoft.AspNetCore.Mvc.ControllerBase).IsAssignableFrom(type)
                && !type.IsAbstract;
        }

        private ActionInfo GetActionInfo(MethodInfo method)
        {
            var httpMethodAttributes = new[]
            {
                "HttpGetAttribute",
                "HttpPostAttribute",
                "HttpPutAttribute",
                "HttpDeleteAttribute",
                "HttpPatchAttribute"
            };

            var httpMethodAttribute = method.GetCustomAttributes()
                .FirstOrDefault(attr => httpMethodAttributes.Contains(attr.GetType().Name));

            if (httpMethodAttribute == null)
            {
                return null; // Not an action method
            }

            var routeAttribute = method.GetCustomAttribute<Microsoft.AspNetCore.Mvc.RouteAttribute>();

            return new ActionInfo
            {
                Name = method.Name,
                HttpMethod = httpMethodAttribute.GetType().Name.Replace("Http", "").Replace("Attribute", ""),
                Route = routeAttribute?.Template ?? string.Empty
            };
        }
    }
}