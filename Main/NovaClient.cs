using Discord.Gateway;
public class NovaClient
{
    private string token;
    private DiscordSocketClient client;
    private string proxyIp;
    private int proxyPort;
    public NovaClient(string token, DiscordSocketClient client, string proxyIp, int proxyPort)
    {
        this.token = token;
        this.client = client;
        this.proxyIp = proxyIp;
        this.proxyPort = proxyPort;
    }
    public string GetToken()
    {
        return token;
    }
    public DiscordSocketClient GetClient()
    {
        return client;
    }
    public string GetProxyIp()
    {
        return proxyIp;
    }
    public int GetProxyPort()
    {
        return proxyPort;
    }
}