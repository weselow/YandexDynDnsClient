// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using YandexDynDnsClient;

Console.WriteLine("Hello, World!");
string file = Path.Combine(AppContext.BaseDirectory,"settings.json");
JsonSerializerOptions DefaultJsonOptions = new() { WriteIndented = true,};
SettingsData sd = LoadSettings();

//нет настроек
if (string.IsNullOrEmpty(sd.Domain))
{
    Console.WriteLine($"No settings - creating file {Path.GetFileName(file)} ...");
    Console.WriteLine("Please, fill data in!");
    AskSettings();
}

HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("Accept", "application/json");
client.DefaultRequestHeaders.Add("PddToken", sd.Token);
string ip = GetIp();

if (sd.RecordId == 0)
{
    //сценарий add
    string requestUrlurl = $"https://pddimp.yandex.ru/api2/admin/dns/add?domain={sd.Domain}&type=A&ttl={sd.Ttl}&content={ip}";
    if (!string.IsNullOrEmpty(sd.Subdomain)) requestUrlurl += $"&subdomain={sd.Subdomain}";

    var result = MakeRequest(url: requestUrlurl);

    if (!string.IsNullOrEmpty(result.Error))
    {
        Console.WriteLine($"Yandex returns Error - {result.Error}");
        Console.ReadKey();
        return;
    }
    else if(result.Success == "ok")
    {
        Console.WriteLine($"Yandex returns success, record id: {result.Record.RecordId}.");
        sd.RecordId = result.Record.RecordId;
        SaveSettings();
        return;
    }
}
else
{
    //сценарий update     
    if(ip == sd.LastIp)
    {
        Console.WriteLine("Ip did not changed.");
        Thread.Sleep(2 * 1000);
        return;
    }

    string requestUrlurl = $"https://pddimp.yandex.ru/api2/admin/dns/edit?domain={sd.Domain}&record_id={sd.RecordId}&ttl={sd.Ttl}&content={ip}";
    if (!string.IsNullOrEmpty(sd.Subdomain)) requestUrlurl += $"&subdomain={sd.Subdomain}";

    var result = MakeRequest(url: requestUrlurl);
    if (!string.IsNullOrEmpty(result.Error))
    {
        Console.WriteLine($"Yandex returns Error - {result.Error}");
        Console.ReadKey();
        return;
    }
    else if (result.Success == "ok")
    {
        Console.WriteLine($"Yandex returns success, record id: {result.Record.RecordId}.");       
        sd.LastIp = ip;
        SaveSettings();
        return;
    }

}

SaveSettings();
Console.WriteLine("Finish!");

bool AskSettings()
{
    bool ifDone = false;

    while (!ifDone)
    {
        Console.WriteLine();
        Console.WriteLine("Starting setup wizard ...");
        Console.Write("Enter domain (e.g. \"example.com\"): ");
        sd.Domain = Console.ReadLine() ?? "";
        Console.WriteLine();

        Console.Write("Enter subdomain if needed (e.g. \"home\" from \"home.example.com\"): ");
        sd.Subdomain = Console.ReadLine() ?? "";
        Console.WriteLine();

        Console.Write($"Enter TTL (default is {sd.Ttl}): ");
        var tmp = Console.ReadLine() ?? "";
        if (!string.IsNullOrEmpty(tmp)) sd.Ttl = Convert.ToInt32(tmp);
        Console.WriteLine();

        Console.Write("Enter Yandex Token : ");
        sd.Token = Console.ReadLine() ?? "";
        Console.WriteLine();

        Console.WriteLine("Is these settings correct? (y/n): ");
        var k = Console.ReadKey();

        if (k.Key == ConsoleKey.Y) { ifDone = true; }
        else { Console.WriteLine("Running wizard again ...");  }
        Console.WriteLine();
    }

    return true;
}


SettingsData LoadSettings()
{
    if(!File.Exists(file)) return new SettingsData();
    try
    {
        var json = File.ReadAllText(file);
        var result = System.Text.Json.JsonSerializer.Deserialize<SettingsData>(json);
        return result ?? new SettingsData(); ;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed to load settings: {e.Message}");       
    }
    return new SettingsData();
}

bool SaveSettings()
{
    try
    {
        var json = System.Text.Json.JsonSerializer.Serialize<SettingsData>(sd, DefaultJsonOptions);
        File.WriteAllText(file, json);
        return true;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed to save settings: {e.Message}");
    }
    return false;
}

string GetIp()
{
    string ip = string.Empty;
    try
    {
        ip = client.GetStringAsync(sd.CheckExternalIpUrl).Result;
    }
    catch (Exception e)
    {
        Console.WriteLine($"Failed to get External ip - {e.Message}");
        throw;
    }
    return ip;
}

Response MakeRequest(string url)
{
    Response? result;
    try
    {
        var response = client.PostAsync(url, null).Result;
        var json = response.Content.ReadAsStringAsync().Result;
        if (string.IsNullOrEmpty(json))
        {
            Console.WriteLine("Json is empty!");
            Console.ReadKey();
            throw new Exception("Json is empty!");
        }
        result = System.Text.Json.JsonSerializer.Deserialize<Response>(json);
        Console.WriteLine("Yandex answer:");
        Console.WriteLine(json);
        Thread.Sleep(2000);
    }
    catch (Exception e)
    {
        Console.WriteLine($"HttpClient returns Error - {e.Message}");
        Console.ReadKey();
        throw;
    }

    if (result == null)
    {
        Console.WriteLine("Result entity is null!");
        Console.ReadKey();
        throw new Exception("Result entity is null!");
    }

    return result;
}
