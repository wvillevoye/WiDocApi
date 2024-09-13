namespace WiDocApi_Blazor.WiDocApi.Helpers
{
    public class HttpMethodClassMapper
    {
        private readonly Dictionary<string, (string buttonClass, string headerClass)> MethodClasses = new()
    {
        { "GET", ("btn-get btn-overall", "accordion-get") },
        { "POST", ("btn-post btn-overall", "accordion-post") },
        { "PUT", ("btn-put btn-overall", "accordion-put") },
        { "PATCH", ("btn-patch btn-overall", "accordion-patch") },
        { "DELETE", ("btn-delete btn-overall", "accordion-delete") },
        { "HEAD", ("btn-head btn-overall", "accordion-head") },
        { "OPTIONS", ("btn-options btn-overall", "accordion-options") }
    };

        public (string buttonClass, string headerClass) GetClasses(string method)
        {
            return MethodClasses.TryGetValue(method.ToUpper(), out var classes)
                ? classes
                : MethodClasses["GET"];
        }
    }
}
