using EventCatalog.Blazor.Models;

namespace EventCatalog.Blazor.Services
{
    public static class Utils
    {
        public static string GenerateIdForNode(object node)
        {
            dynamic data = node;
            return $"{data.Id}-{data.Version}";
        }

        public static string GenerateIdForEdge(object source, object target)
        {
            dynamic sourceData = source;
            dynamic targetData = target;
            return $"{sourceData.Id}-{sourceData.Version}-{targetData.Id}-{targetData.Version}";
        }
    }
}
