public class Script: ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        await Task.CompletedTask;
        return Context.OperationId switch
        {
            "Hash" => Hash(),
            _ => new HttpResponseMessage(HttpStatusCode.BadRequest)
        };
    }

    private HttpResponseMessage Hash()
    {
        var query = HttpUtility.ParseQueryString(Context.Request.RequestUri?.Query ?? string.Empty);
        var value = query.AllKeys.Any(_ => _ == "value") 
            ? query["value"] : string.Empty;

        var hashedString = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(value));
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent((new JObject
        {
            ["value"] = value,
            ["Hashed"] = hashedString,
        }).ToString());
        return response;
    }
}