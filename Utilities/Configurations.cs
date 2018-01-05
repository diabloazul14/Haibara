using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class Configurations
{
    public static Dictionary<string, string> ReturnConfigurations() 
    {   
        List<string> configurationList = new List<string>(){"SecurityKey", "PasswordSalt"};
        Dictionary<string, string> configurations = new Dictionary<string, string>();
        string configurationFileJsonString = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/appconfigurations.json");
        JObject configurationJObject = JObject.Parse(configurationFileJsonString);
        foreach(string configuration in configurationList)
            configurations.Add(configuration, configurationJObject[configuration].ToString());
        return configurations;
    }
}