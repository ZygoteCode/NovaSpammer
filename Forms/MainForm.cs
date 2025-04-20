using System.Windows.Forms;
using System.Diagnostics;
using System;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Discord.Gateway;
using Discord;
using Discord.Media;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
public partial class MainForm : Form
{
    public static Random rand = new Random();
    public static bool dmSpammerWorking, serverSpammerWorking, typingSpammerWorking, tokenCheckerWorking;
    public static List<DiscordVoiceSession> sessions;
    public static List<NovaClient> novaClients;
    public MainForm()
    {
        InitializeComponent();
        CheckForIllegalCrossThreadCalls = false;
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
        openFileDialog1.DefaultExt = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        if (!System.IO.File.Exists("tokens.txt"))
        {
            System.IO.File.WriteAllText("tokens.txt", "");
        }
        else
        {
            textBox1.Text = System.IO.File.ReadAllText("tokens.txt");
        }
        if (!System.IO.File.Exists("proxies.txt"))
        {
            System.IO.File.WriteAllText("proxies.txt", "");
        }
        else
        {
            textBox2.Text = System.IO.File.ReadAllText("proxies.txt");
        }
        sessions = new List<DiscordVoiceSession>();
        novaClients = new List<NovaClient>();
        comboBox1.SelectedIndex = 0;
    }
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }
    private void firefoxButton1_Click(object sender, EventArgs e)
    {
        openFileDialog1.Title = "Load your tokens here...";
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            textBox1.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
        }
    }
    private void firefoxButton2_Click(object sender, EventArgs e)
    {
        openFileDialog1.Title = "Load your proxies here...";
        if (openFileDialog1.ShowDialog() == DialogResult.OK)
        {
            textBox2.Text = System.IO.File.ReadAllText(openFileDialog1.FileName);
        }
    }
    private void firefoxButton3_Click(object sender, EventArgs e)
    {
        if (firefoxCheckBox1.Checked)
        {
            try
            {
                Thread thread = new Thread(() => doServerJoinerProxy(textBox3.Text));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
        else
        {
            try
            {
                Thread thread = new Thread(() => doServerJoiner(textBox3.Text));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
    }
    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        label2.Text = "Delay: " + trackBar1.Value.ToString() + "ms";
    }
    public void doServerJoiner(string invite)
    {
        invite = invite.Replace("https://discord.gg/", "").Replace("https://discord.com/", "").Replace("https://discord.com/invite/", "").Replace("https://discordapp.com/invite/", "").Replace(" ", "");
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar1.Value);
                    Thread thread = new Thread(() => joinServer(invite, client.GetToken()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void joinServer(string invite, string token)
    {
        PostRequest(token, "https://discord.com/api/v8/science");
        PostRequest(token, "https://discord.com/api/v8/science");
        GetRequest(token, "https://discord.com/api/v8/invites/" + invite + "?inputValue=" + invite + "&with_counts=true");
        PostRequest(token, "https://discord.com/api/v8/science");
        var http = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://discord.com/api/v8/invites/" + invite), Method = HttpMethod.Post, Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" },},
            };
            http.SendAsync(request);
        }
        catch (Exception )
        {
        }
        PostRequest(token, "https://discord.com/api/v8/science");
        PostRequest(token, "https://discord.com/api/v8/science");
    }
    public void doServerJoinerProxy(string invite)
    {
        invite = invite.Replace("https://discord.gg/", "").Replace("https://discord.com/", "").Replace("https://discord.com/invite/", "").Replace("https://discordapp.com/invite/", "").Replace(" ", "");
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar1.Value);
                    Thread thread = new Thread(() => joinServerProxy(invite, client));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void joinServerProxy(string invite, NovaClient client)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.PreAuthenticate = false;
        handler.UseProxy = true;
        handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
        var http = new HttpClient(handler);
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        GetRequest(client.GetToken(), "https://discord.com/api/v8/invites/" + invite + "?inputValue=" + invite + "&with_counts=true", client.GetProxyIp(), client.GetProxyPort());
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://discord.com/api/v8/invites/" + invite),
                Method = HttpMethod.Post,
                Headers = { { HttpRequestHeader.Authorization.ToString(), client.GetToken() }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
    }
    private void firefoxButton4_Click(object sender, EventArgs e)
    {
        if (firefoxCheckBox1.Checked)
        {
            try
            {
                Thread thread = new Thread(() => doServerLeaverProxy(ulong.Parse(textBox4.Text)));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
        else
        {
            try
            {
                Thread thread = new Thread(() => doServerLeaver(ulong.Parse(textBox4.Text)));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
    }
    private void trackBar2_Scroll(object sender, EventArgs e)
    {
        label1.Text = "Delay: " + trackBar2.Value.ToString() + "ms";
    }
    public void doServerLeaver(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar2.Value);
                    Thread thread = new Thread(() => leaveServer(id, client.GetToken()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void leaveServer(ulong id, string token)
    {
        var http = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/users/@me/guilds/" + id.ToString()),
                Method = HttpMethod.Delete,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    public void doServerLeaverProxy(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar2.Value);
                    Thread thread = new Thread(() => leaveServerProxy(id, client));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void leaveServerProxy(ulong id, NovaClient client)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.PreAuthenticate = false;
        handler.UseProxy = true;
        handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
        var http = new HttpClient(handler);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/users/@me/guilds/" + id.ToString()),
                Method = HttpMethod.Delete,
                Headers = { { HttpRequestHeader.Authorization.ToString(), client.GetToken() }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    private void trackBar3_Scroll(object sender, EventArgs e)
    {
        label3.Text = "Delay: " + trackBar3.Value.ToString() + "ms";
    }
    private void firefoxButton5_Click(object sender, EventArgs e)
    {
        if (firefoxCheckBox1.Checked)
        {
            try
            {
                Thread thread = new Thread(() => doFriendAdderProxy(ulong.Parse(textBox5.Text)));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
        else
        {
            try
            {
                Thread thread = new Thread(() => doFriendAdder(ulong.Parse(textBox5.Text)));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
    }
    public void doFriendAdder(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar3.Value);
                    Thread thread = new Thread(() => addFriend(id, client.GetToken()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void addFriend(ulong id, string token)
    {
        PostRequest(token, "https://discord.com/api/v8/science");
        GetRequest(token, "https://discord.com/api/v8/users/@me/notes/" + id.ToString());
        var http = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/users/@me/relationships/" + id.ToString()),
                Method = HttpMethod.Put,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    public void doFriendAdderProxy(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar3.Value);
                    Thread thread = new Thread(() => addFriendProxy(id, client));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void addFriendProxy(ulong id, NovaClient client)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.PreAuthenticate = false;
        handler.UseProxy = true;
        handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        GetRequest(client.GetToken(), "https://discord.com/api/v8/users/@me/notes/" + id.ToString(), client.GetProxyIp(), client.GetProxyPort());
        var http = new HttpClient(handler);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/users/@me/relationships/" + id.ToString()),
                Method = HttpMethod.Put,
                Headers = { { HttpRequestHeader.Authorization.ToString(), client.GetToken() }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton6_Click(object sender, EventArgs e)
    {
        if (firefoxCheckBox1.Checked)
        {
            try
            {
                Thread thread = new Thread(() => doFriendRemoverProxy(ulong.Parse(textBox5.Text)));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
        else
        {
            try
            {
                Thread thread = new Thread(() => doFriendRemover(ulong.Parse(textBox5.Text)));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
    }
    public void doFriendRemover(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar3.Value);
                    Thread thread = new Thread(() => removeFriend(id, client.GetToken()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void removeFriend(ulong id, string token)
    {
        PostRequest(token, "https://discord.com/api/v8/science");
        var http = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/users/@me/relationships/" + id.ToString()),
                Method = HttpMethod.Delete,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    public void doFriendRemoverProxy(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar3.Value);
                    Thread thread = new Thread(() => removeFriendProxy(id, client));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void removeFriendProxy(ulong id, NovaClient client)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.PreAuthenticate = false;
        handler.UseProxy = true;
        handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
        var http = new HttpClient(handler);
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/users/@me/relationships/" + id.ToString()),
                Method = HttpMethod.Delete,
                Headers = { { HttpRequestHeader.Authorization.ToString(), client.GetToken() }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    private void trackBar4_Scroll(object sender, EventArgs e)
    {
        label4.Text = "Delay: " + trackBar4.Value.ToString() + "ms";
    }
    private void firefoxButton8_Click(object sender, EventArgs e)
    {
        try
        {
            firefoxButton8.Enabled = false;
            dmSpammerWorking = true;
            if (firefoxCheckBox1.Checked)
            {
                try
                {
                    Thread thread = new Thread(() => doDMSpammerProxy(ulong.Parse(textBox6.Text)));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    Thread thread = new Thread(() => doDMSpammer(ulong.Parse(textBox6.Text)));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
            firefoxButton7.Enabled = true;
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton7_Click(object sender, EventArgs e)
    {
        try
        {
            firefoxButton7.Enabled = false;
            dmSpammerWorking = false;
            firefoxButton8.Enabled = true;
        }
        catch (Exception)
        {
        }
    }
    public void doDMSpammerProxy(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    for (int i = 0; i < trackBar9.Value; i++)
                    {
                        try
                        {
                            Thread thread = new Thread(() => dmSpamProxy(id, client.GetToken(), client));
                            thread.Priority = ThreadPriority.Highest;
                            thread.Start();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void dmSpamProxy(ulong id, string token, NovaClient client)
    {
        try
        {
            string recipient = createDMProxy(token, id.ToString(), new WebProxy(client.GetProxyIp(), client.GetProxyPort()), 5000).Result;
            string msg = "";
            if (firefoxCheckBox2.Checked)
            {
                msg = ">>> ";
            }
            if (textBox7.Lines.Length != 1)
            {
                foreach (string line in textBox7.Lines)
                {
                    msg = msg + " \\u000d" + line;
                }
            }
            else
            {
                msg += textBox7.Text;
            }
            msg += " " + rand.Next(1000, 9999);
            if (recipient != null && recipient != "")
            {
                GetRequest(token, "https://discord.com/api/v8/channels/" + recipient + "/messages?limit=50", client.GetProxyIp(), client.GetProxyPort());
                PostRequest(token, "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
                while (true)
                {
                    if (dmSpammerWorking)
                    {
                        Thread.Sleep(trackBar4.Value);
                        try
                        {
                            var handler = new HttpClientHandler();
                            handler.UseProxy = true;
                            handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
                            var http = new HttpClient(handler);
                            string messageJson = "";
                            messageJson = "{\"content\":\"" + msg + "\"}";
                            var str_con = new StringContent(messageJson, Encoding.UTF8, "application/json");
                            PostRequest(token, "https://discord.com/api/v8/channels/" + recipient + "/typing", client.GetProxyIp(), client.GetProxyPort());
                            var authReq = new HttpRequestMessage
                            {

                                RequestUri = new Uri("https://discord.com/api/v8/channels/" + recipient + "/messages"),
                                Content = str_con,
                                Headers = {
                            { HttpRequestHeader.ContentType.ToString(), "application/json" },
                                                    { HttpRequestHeader.Authorization.ToString(), token }
                    },
                                Method = HttpMethod.Post
                            };
                            http.SendAsync(authReq);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void doDMSpammer(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    for (int i = 0; i < trackBar9.Value; i++)
                    {
                        try
                        {
                            Thread thread = new Thread(() => dmSpam(id, client.GetToken()));
                            thread.Priority = ThreadPriority.Highest;
                            thread.Start();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void dmSpam(ulong id, string token)
    {
        try
        {
            string recipient = createDM(token, id.ToString(), 5000).Result;
            string msg = "";
            if (firefoxCheckBox2.Checked)
            {
                msg = ">>> ";
            }
            if (textBox7.Lines.Length != 1)
            {
                foreach (string line in textBox7.Lines)
                {
                    msg = msg + " \\u000d" + line;
                }
            }
            else
            {
                msg += textBox7.Text;
            }
            msg += " " + rand.Next(1000, 9999);
            if (recipient != null && recipient != "")
            {
                GetRequest(token, "https://discord.com/api/v8/channels/" + id.ToString() + "/messages?limit=50");
                PostRequest(token, "https://discord.com/api/v8/science");
                while (true)
                {
                    if (dmSpammerWorking)
                    {
                        Thread.Sleep(trackBar4.Value);
                        try
                        {
                            var http = new HttpClient();
                            string messageJson = "";
                            messageJson = "{\"content\":\"" + msg + "\"}";
                            var str_con = new StringContent(messageJson, Encoding.UTF8, "application/json");
                            PostRequest(token, "https://discord.com/api/v8/channels/" + recipient + "/typing");
                            var authReq = new HttpRequestMessage
                            {

                                RequestUri = new Uri("https://discord.com/api/v8/channels/" + recipient + "/messages"),
                                Content = str_con,
                                Headers = {
                            { HttpRequestHeader.ContentType.ToString(), "application/json" },
                                                    { HttpRequestHeader.Authorization.ToString(), token }
                    },
                                Method = HttpMethod.Post
                            };
                            http.SendAsync(authReq);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public async Task<String> createDMProxy(string token, string targetUserId, WebProxy webProxy, int timeout = 5000)
    {
        try
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.PreAuthenticate = false;
            handler.UseProxy = true;
            handler.Proxy = webProxy;
            var http = new HttpClient(handler);
            http.Timeout = TimeSpan.FromMilliseconds(timeout);
            var requestOpen = new HttpRequestMessage
            {

                RequestUri = new Uri("https://discord.com/api/v8/users/@me/channels"),
                Method = HttpMethod.Post,
                Headers =
                      {
                    { HttpRequestHeader.Authorization.ToString(), token },
                    { HttpRequestHeader.ContentType.ToString(), "application/json" }
                    },
            };
            requestOpen.Content = new StringContent("{\"recipient_id\": " + targetUserId + "}", Encoding.UTF8, "application/json");
            var requestOpenResponse = await http.SendAsync(requestOpen);
            var responseStr = await requestOpenResponse.Content.ReadAsStringAsync();
            if (responseStr.Contains("401") || responseStr.Contains("403") || responseStr.Contains("500") || responseStr.Contains("verify") || responseStr.Contains("400"))
            {
                return "";
            }
            dynamic jss = JObject.Parse(responseStr);
            var channelId = jss.id;
            return channelId;
        }
        catch (Exception)
        {
            return "";
        }
    }
    public async Task<String> createDM(string token, string targetUserId, int timeout = 5000)
    {
        try
        {
            var http = new HttpClient();
            http.Timeout = TimeSpan.FromMilliseconds(timeout);
            var requestOpen = new HttpRequestMessage
            {

                RequestUri = new Uri("https://discord.com/api/v8/users/@me/channels"),
                Method = HttpMethod.Post,
                Headers =
                      {
                    { HttpRequestHeader.Authorization.ToString(), token },
                    { HttpRequestHeader.ContentType.ToString(), "application/json" }
                    },
            };
            requestOpen.Content = new StringContent("{\"recipient_id\": " + targetUserId + "}", Encoding.UTF8, "application/json");
            var requestOpenResponse = await http.SendAsync(requestOpen);
            var responseStr = await requestOpenResponse.Content.ReadAsStringAsync();
            if (responseStr.Contains("401") || responseStr.Contains("403") || responseStr.Contains("500") || responseStr.Contains("verify") || responseStr.Contains("400"))
            {
                return "";
            }
            dynamic jss = JObject.Parse(responseStr);
            var channelId = jss.id;
            return channelId;
        }
        catch (Exception)
        {
            return "";
        }
    }
    private void trackBar5_Scroll(object sender, EventArgs e)
    {
        label5.Text = "Delay: " + trackBar5.Value.ToString() + "ms";
    }
    private void firefoxButton10_Click(object sender, EventArgs e)
    {
        try
        {
            firefoxButton10.Enabled = false;
            serverSpammerWorking = true;
            if (firefoxCheckBox1.Checked)
            {
                try
                {
                    Thread thread = new Thread(() => doServerSpamProxy(ulong.Parse(textBox8.Text)));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    Thread thread = new Thread(() => doServerSpammer(ulong.Parse(textBox8.Text)));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
            firefoxButton9.Enabled = true;
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton9_Click(object sender, EventArgs e)
    {
        try
        {
            firefoxButton9.Enabled = false;
            serverSpammerWorking = false;
            firefoxButton10.Enabled = true;
        }
        catch (Exception)
        {
        }
    }
    public void doServerSpamProxy(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    GetRequest(client.GetToken(), "https://discord.com/api/v8/channels/" + id.ToString() + "/messages?limit=50", client.GetProxyIp(), client.GetProxyPort());
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
                    for (int i = 0; i < trackBar8.Value; i++)
                    {
                        try
                        {
                            Thread thread = new Thread(() => serverSpamProxy(id, client.GetToken(), client));
                            thread.Priority = ThreadPriority.Highest;
                            thread.Start();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void serverSpamProxy(ulong id, string token, NovaClient client)
    {
        try
        {
            string msg = "";
            while (true)
            {
                if (serverSpammerWorking)
                {
                    Thread.Sleep(trackBar5.Value);
                    try
                    {
                        msg = "";
                        if (firefoxCheckBox3.Checked)
                        {
                            msg = ">>> ";
                        }
                        if (textBox9.Lines.Length != 1)
                        {
                            foreach (string line in textBox9.Lines)
                            {
                                msg = msg + " \\u000d" + line;
                            }
                        }
                        else
                        {
                            msg += textBox9.Text;
                        }
                        if (firefoxCheckBox5.Checked)
                        {
                            msg += " " + rand.Next(1000, 9999);
                        }
                        var handler = new HttpClientHandler();
                        handler.UseProxy = true;
                        handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
                        var http = new HttpClient(handler);
                        string messageJson = "";
                        if (firefoxCheckBox4.Checked)
                        {
                            messageJson = "{\"content\":\"" + msg + "\", \"tts\": true}";
                        }
                        else
                        {
                            messageJson = "{\"content\":\"" + msg + "\"}";
                        }
                        PostRequest(token, "https://discord.com/api/v8/channels/" + id.ToString() + "/typing", client.GetProxyIp(), client.GetProxyPort());
                        var authReq = new HttpRequestMessage
                        {

                            RequestUri = new Uri("https://discord.com/api/v8/channels/" + id.ToString() + "/messages"),
                            Content = new StringContent(messageJson, Encoding.UTF8, "application/json"),
                            Headers = {
                            { HttpRequestHeader.ContentType.ToString(), "application/json" },
                                                    { HttpRequestHeader.Authorization.ToString(), token }
                    },
                            Method = HttpMethod.Post
                        };
                        http.SendAsync(authReq);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void doServerSpammer(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    GetRequest(client.GetToken(), "https://discord.com/api/v8/channels/" + id.ToString() + "/messages?limit=50");
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science");
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science");
                    for (int i = 0; i < trackBar8.Value; i++)
                    {
                        try
                        {
                            Thread thread = new Thread(() => serverSpam(id, client.GetToken()));
                            thread.Priority = ThreadPriority.Highest;
                            thread.Start();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void serverSpam(ulong id, string token)
    {
        try
        {
            string msg = "";
            while (true)
            {
                if (serverSpammerWorking)
                {
                    Thread.Sleep(trackBar5.Value);
                    try
                    {
                        msg = "";
                        if (firefoxCheckBox3.Checked)
                        {
                            msg = ">>> ";
                        }
                        if (textBox9.Lines.Length != 1)
                        {
                            foreach (string line in textBox9.Lines)
                            {
                                msg = msg + " \\u000d" + line;
                            }
                        }
                        else
                        {
                            msg += textBox9.Text;
                        }
                        if (firefoxCheckBox5.Checked)
                        {
                            msg += " " + rand.Next(1000, 9999);
                        }
                        var http = new HttpClient();
                        string messageJson = "";
                        if (firefoxCheckBox4.Checked)
                        {
                            messageJson = "{\"content\":\"" + msg + "\", \"tts\": true}";
                        }
                        else
                        {
                            messageJson = "{\"content\":\"" + msg + "\"}";
                        }
                        PostRequest(token, "https://discord.com/api/v8/channels/" + id.ToString() + "/typing");
                        var authReq = new HttpRequestMessage
                        {

                            RequestUri = new Uri("https://discord.com/api/v8/channels/" + id.ToString() + "/messages"),
                            Content = new StringContent(messageJson, Encoding.UTF8, "application/json"),
                            Headers = {
                            { HttpRequestHeader.ContentType.ToString(), "application/json" },
                                                    { HttpRequestHeader.Authorization.ToString(), token }
                    },
                            Method = HttpMethod.Post
                        };
                        http.SendAsync(authReq);
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    return;
                }
            }
        }
        catch (Exception)
        {
        }
    }
    private void trackBar6_Scroll(object sender, EventArgs e)
    {
        label6.Text = "Delay: " + trackBar6.Value.ToString() + "ms";
    }
    private void firefoxButton14_Click(object sender, EventArgs e)
    {
        try
        {
            firefoxButton14.Enabled = false;
            typingSpammerWorking = true;
            if (firefoxCheckBox1.Checked)
            {
                try
                {
                    Thread thread = new Thread(() => doTypingSpammerProxy(ulong.Parse(textBox13.Text)));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    Thread thread = new Thread(() => doTypingSpammer(ulong.Parse(textBox13.Text)));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
            firefoxButton13.Enabled = true;
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton13_Click(object sender, EventArgs e)
    {
        try
        {
            firefoxButton13.Enabled = false;
            typingSpammerWorking = false;
            firefoxButton14.Enabled = true;
        }
        catch (Exception)
        {
        }
    }
    public void doTypingSpammer(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar7.Value);
                    GetRequest(client.GetToken(), "https://discord.com/api/v8/channels/" + id.ToString() + "/messages?limit=50");
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science");
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science");
                    Thread thread = new Thread(() => typingSpam(id, client.GetToken()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void typingSpam(ulong id, string token)
    {
        while (true)
        {
            if (typingSpammerWorking)
            {
                var http = new HttpClient();
                try
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri("https://discord.com/api/v8/channels/" + id.ToString() + "/typing"),
                        Method = HttpMethod.Post,
                        Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
                    };
                    http.SendAsync(request);
                }
                catch (Exception)
                {
                }
                Thread.Sleep(8000);
            }
            else
            {
                return;
            }
        }
    }
    private void trackBar7_Scroll(object sender, EventArgs e)
    {
        label7.Text = "Delay: " + trackBar7.Value.ToString() + "ms";
    }
    public void doTypingSpammerProxy(ulong id)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar7.Value);
                    Thread thread = new Thread(() => typingSpamProxy(id, client));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void typingSpamProxy(ulong id, NovaClient client)
    {
        var proxy = textBox2.Lines[rand.Next(textBox2.Lines.Length)].Split(':');
        while (true)
        {
            if (typingSpammerWorking)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.PreAuthenticate = false;
                handler.UseProxy = true;
                handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
                GetRequest(client.GetToken(), "https://discord.com/api/v8/channels/" + id.ToString() + "/messages?limit=50", client.GetProxyIp(), client.GetProxyPort());
                PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
                PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
                var http = new HttpClient(handler);
                try
                {
                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri("https://discord.com/api/v8/channels/" + id.ToString() + "/typing"),
                        Method = HttpMethod.Post,
                        Headers = { { HttpRequestHeader.Authorization.ToString(), client.GetToken() }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
                    };
                    http.SendAsync(request);
                }
                catch (Exception)
                {
                }
                Thread.Sleep(8000);
            }
            else
            {
                return;
            }
        }
    }
    private void firefoxButton12_Click(object sender, EventArgs e)
    {
        if (firefoxCheckBox1.Checked)
        {
            try
            {
                Thread thread = new Thread(() => doReactionAdderProxy(ulong.Parse(textBox10.Text), ulong.Parse(textBox11.Text), textBox12.Text));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
        else
        {
            try
            {
                Thread thread = new Thread(() => doReactionAdder(ulong.Parse(textBox10.Text), ulong.Parse(textBox11.Text), textBox12.Text));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
    }
    public void doReactionAdder(ulong channelId, ulong messageId, string emoji)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar6.Value);
                    GetRequest(client.GetToken(), "https://discord.com/api/v8/channels/" + channelId.ToString() + "/messages?limit=50");
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science");
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science");
                    Thread thread = new Thread(() => addReaction(channelId, messageId, emoji, client.GetToken()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void addReaction(ulong channelId, ulong messageId, string emoji, string token)
    {
        var http = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/channels/" + channelId.ToString() + "/messages/" + messageId.ToString() + "/reactions/" + emoji + "/@me"),
                Method = HttpMethod.Put,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    public void doReactionAdderProxy(ulong channelId, ulong messageId, string emoji)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar2.Value);
                    Thread thread = new Thread(() => addReactionProxy(channelId, messageId, emoji, client));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void addReactionProxy(ulong channelId, ulong messageId, string emoji, NovaClient client)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.PreAuthenticate = false;
        handler.UseProxy = true;
        handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
        GetRequest(client.GetToken(), "https://discord.com/api/v8/channels/" + channelId.ToString() + "/messages?limit=50", client.GetProxyIp(), client.GetProxyPort());
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        var http = new HttpClient(handler);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/channels/" + channelId.ToString() + "/messages/" + messageId.ToString() + "/reactions/" + emoji + "/@me"),
                Method = HttpMethod.Put,
                Headers = { { HttpRequestHeader.Authorization.ToString(), client.GetToken() }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton11_Click(object sender, EventArgs e)
    {
        if (firefoxCheckBox1.Checked)
        {
            try
            {
                Thread thread = new Thread(() => doReactionRemoverProxy(ulong.Parse(textBox10.Text), ulong.Parse(textBox11.Text), textBox12.Text));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
        else
        {
            try
            {
                Thread thread = new Thread(() => doReactionRemover(ulong.Parse(textBox10.Text), ulong.Parse(textBox11.Text), textBox12.Text));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
    }
    public void doReactionRemover(ulong channelId, ulong messageId, string emoji)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar6.Value);
                    GetRequest(client.GetToken(), "https://discord.com/api/v8/channels/" + channelId.ToString() + "/messages?limit=50");
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science");
                    PostRequest(client.GetToken(), "https://discord.com/api/v8/science");
                    Thread thread = new Thread(() => removeReaction(channelId, messageId, emoji, client.GetToken()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void removeReaction(ulong channelId, ulong messageId, string emoji, string token)
    {
        var http = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/channels/" + channelId.ToString() + "/messages/" + messageId.ToString() + "/reactions/" + emoji + "/@me"),
                Method = HttpMethod.Delete,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    private void textBox1_TextChanged(object sender, EventArgs e)
    {
        System.IO.File.WriteAllText("tokens.txt", textBox1.Text);
    }
    private void textBox2_TextChanged(object sender, EventArgs e)
    {
        System.IO.File.WriteAllText("proxies.txt", textBox2.Text);
    }
    private void trackBar8_Scroll(object sender, EventArgs e)
    {
        label8.Text = "Number of threads per token: " + trackBar8.Value.ToString();
    }
    private void trackBar9_Scroll(object sender, EventArgs e)
    {
        label9.Text = "Number of threads per token: " + trackBar9.Value.ToString();
    }
    private void firefoxButton15_Click(object sender, EventArgs e)
    {
        if (firefoxCheckBox1.Checked)
        {
            if (textBox2.Lines.Length < textBox1.Lines.Length)
            {
                MessageBox.Show("You need a number of proxies equivalent/higher to the number of tokens!", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    Thread thread = new Thread(() => LoadTokens());
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                    MessageBox.Show("Succesfully loaded all tokens!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                }
            }
        }
        else
        {
            try
            {
                Thread thread = new Thread(() => LoadTokens());
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
                MessageBox.Show("Succesfully loaded all tokens!", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
            }
        }
    }
    public void LoadTokens()
    {
        int i = 0;
        foreach (string token in textBox1.Lines)
        {
            try
            {
                Thread thread = new Thread(() => LoadToken(token, i));
                thread.Priority = ThreadPriority.Highest;
                thread.Start();
            }
            catch (Exception)
            {
            }
            i++;
        }
    }
    public void LoadToken(string token, int proxy)
    {
        try
        {
            if (firefoxCheckBox1.Checked)
            {
                string proxyy = textBox2.Lines[proxy];
                string ip = proxyy.Split(':')[0];
                int port = int.Parse(proxyy.Split(':')[1]);
                try
                {
                    GetRequest(token, "https://discord.com/api/v8/users/@me/billing/trials/520373071933079552/eligibility", ip, port);
                    GetRequest(token, "https://discord.com/api/v8/users/@me/affinities/guilds", ip, port);
                    GetRequest(token, "https://discord.com/api/v2/scheduled-maintenances/upcoming.json", ip, port);
                    GetRequest(token, "https://discord.com/api/v8/users/@me/library", ip, port);
                    GetRequest(token, "https://discord.com/api/v8/applications/detectable", ip, port);
                    GetRequest(token, "https://discord.com/api/v8/users/@me/affinities/users", ip, port);
                    GetRequest(token, "https://discord.com/api/v8/science", ip, port);
                    GetRequest(token, "https://discord.com/api/v8/users/@me/relationships", ip, port);
                    GetRequest(token, "https://discord.com/api/v8/science", ip, port);
                    GetRequest(token, "https://discord.com/api/v8/detectable", ip, port);
                    DiscordSocketConfig discordSocketConfig = new DiscordSocketConfig();
                    AnarchyProxy anarchyProxy = new AnarchyProxy();
                    anarchyProxy.Parse(proxyy);
                    discordSocketConfig.Proxy = anarchyProxy;
                    DiscordSocketClient discordSocketClient = new DiscordSocketClient(discordSocketConfig);
                    discordSocketClient.Login(token);
                    novaClients.Add(new NovaClient(token, discordSocketClient, ip, port));
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    GetRequest(token, "https://discord.com/api/v8/users/@me/billing/trials/520373071933079552/eligibility");
                    GetRequest(token, "https://discord.com/api/v8/users/@me/affinities/guilds");
                    GetRequest(token, "https://discord.com/api/v2/scheduled-maintenances/upcoming.json");
                    GetRequest(token, "https://discord.com/api/v8/users/@me/library");
                    GetRequest(token, "https://discord.com/api/v8/applications/detectable");
                    GetRequest(token, "https://discord.com/api/v8/users/@me/affinities/users");
                    GetRequest(token, "https://discord.com/api/v8/science");
                    GetRequest(token, "https://discord.com/api/v8/users/@me/relationships");
                    GetRequest(token, "https://discord.com/api/v8/science");
                    GetRequest(token, "https://discord.com/api/v8/detectable");
                    DiscordSocketClient discordSocketClient = new DiscordSocketClient(null);
                    discordSocketClient.Login(token);
                    novaClients.Add(new NovaClient(token, discordSocketClient, "", 0));
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void GetRequest(string token, string link)
    {
        var http = new HttpClient();
        try
        {   
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(link),
                Method = HttpMethod.Get,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    public void GetRequest(string token, string link, string ip, int port)
    {
        var handler = new HttpClientHandler();
        handler.PreAuthenticate = false;
        handler.UseProxy = true;
        handler.Proxy = new WebProxy(ip, port);
        var http = new HttpClient(handler);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(link),
                Method = HttpMethod.Get,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    public void PostRequest(string token, string link)
    {
        var http = new HttpClient();
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(link),
                Method = HttpMethod.Post,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton16_Click(object sender, EventArgs e)
    {
        if (openFileDialog2.ShowDialog() == DialogResult.OK)
        {
            textBox16.Text = openFileDialog2.FileName;
        }
    }
    public void PostRequest(string token, string link, string ip, int port)
    {
        var handler = new HttpClientHandler();
        handler.PreAuthenticate = false;
        handler.UseProxy = true;
        handler.Proxy = new WebProxy(ip, port);
        var http = new HttpClient(handler);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(link),
                Method = HttpMethod.Post,
                Headers = { { HttpRequestHeader.Authorization.ToString(), token }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    private void trackBar10_Scroll(object sender, EventArgs e)
    {
        label16.Text = "Delay: " + trackBar10.Value.ToString() + "ms";
    }
    private void trackBar11_Scroll(object sender, EventArgs e)
    {
        label10.Text = "Delay: " + trackBar11.Value.ToString() + "ms";
    }
    public void doReactionRemoverProxy(ulong channelId, ulong messageId, string emoji)
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar2.Value);
                    Thread thread = new Thread(() => removeReactionProxy(channelId, messageId, emoji, client));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void removeReactionProxy(ulong channelId, ulong messageId, string emoji, NovaClient client)
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.PreAuthenticate = false;
        handler.UseProxy = true;
        handler.Proxy = new WebProxy(client.GetProxyIp(), client.GetProxyPort());
        GetRequest(client.GetToken(), "https://discord.com/api/v8/channels/" + channelId.ToString() + "/messages?limit=50", client.GetProxyIp(), client.GetProxyPort());
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        PostRequest(client.GetToken(), "https://discord.com/api/v8/science", client.GetProxyIp(), client.GetProxyPort());
        var http = new HttpClient(handler);
        try
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"https://discord.com/api/v8/channels/" + channelId.ToString() + "/messages/" + messageId.ToString() + "/reactions/" + emoji + "/@me"),
                Method = HttpMethod.Delete,
                Headers = { { HttpRequestHeader.Authorization.ToString(), client.GetToken() }, { HttpRequestHeader.ContentType.ToString(), "multipart/mixed" }, },
            };
            http.SendAsync(request);
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton17_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(() => doVoiceJoiner());
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }
        catch (Exception)
        {
        }
    }
    public void doVoiceJoiner()
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar10.Value);
                    Thread thread = new Thread(() => joinVoice(client.GetClient()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void joinVoice(DiscordSocketClient client)
    {
        try
        {
            DiscordVoiceSession session = client.JoinVoiceChannel(new VoiceStateProperties() { ChannelId = ulong.Parse(textBox14.Text), GuildId = ulong.Parse(textBox15.Text), Muted = firefoxCheckBox19.Checked, Deafened = firefoxCheckBox20.Checked, Video = firefoxCheckBox8.Checked });
            session.ReceivePackets = false;
            session.OnConnected += Session_OnConnected;
            session.Connect();
            sessions.Add(session);
            if (firefoxCheckBox11.Checked)
            {
                Thread.Sleep(trackBar11.Value);
                session.Disconnect();
            }
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton18_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(() => doVoiceLefter());
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }
        catch (Exception)
        {
        }
    }
    public void doVoiceLefter()
    {
        try
        {
            foreach (DiscordVoiceSession session in sessions)
            {
                try
                {
                    Thread.Sleep(trackBar10.Value);
                    Thread thread = new Thread(() => leftVoice(session));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void leftVoice(DiscordVoiceSession session)
    {
        for (int i = 0; i < 3; i++)
        {
            Thread.Sleep(250);
            try
            {
                session.Disconnect();
            }
            catch (Exception)
            {
            }
            try
            {
                sessions.Remove(session);
            }
            catch (Exception)
            {
            }
        }
    }
    public void goLive(DiscordVoiceSession session)
    {
        try
        {
            session.GoLive();
        }
        catch (Exception ex)
        {
        }
    }
    public void watchGoLive(DiscordVoiceSession session)
    {
        try
        {
            session.WatchGoLive(ulong.Parse(textBox17.Text));
        }
        catch (Exception ex)
        {
        }
    }
    private void Session_OnConnected(DiscordVoiceSession session, EventArgs e)
    {
        if (firefoxCheckBox6.Checked)
        {
            try
            {
                new Thread(() => goLive(session)).Start();
            }
            catch (Exception ex)
            {
            }
        }
        if (firefoxCheckBox7.Checked)
        {
            try
            {
                new Thread(() => watchGoLive(session)).Start();
            }
            catch (Exception ex)
            {
            }
        }
        if (firefoxCheckBox18.Checked)
        {
            DiscordVoiceStream discordVoiceStream = session.CreateStream(96000u, Discord.Media.AudioApplication.Mixed);
            session.SetSpeakingState(DiscordSpeakingFlags.Soundshare);
            if (firefoxRadioButton6.Checked)
            {
                for (int i = 0; i < numericUpDown12.Value; i++)
                {
                    if (System.IO.File.Exists(textBox16.Text))
                    {
                        try
                        {
                            discordVoiceStream.CopyFrom(DiscordVoiceUtils.GetAudioStream(textBox16.Text));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
            else
            {
                for (;;)
                {
                    if (System.IO.File.Exists(textBox16.Text))
                    {
                        try
                        {
                            discordVoiceStream.CopyFrom(DiscordVoiceUtils.GetAudioStream(textBox16.Text));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            }
        }
    }
    private void firefoxButton19_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(() => doGameSet());
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }
        catch (Exception)
        {
        }
    }
    private void firefoxButton20_Click(object sender, EventArgs e)
    {
        try
        {
            Thread thread = new Thread(() => doSetOnlineStatus());
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }
        catch (Exception)
        {
        }
    }
    public void doGameSet()
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar10.Value);
                    Thread thread = new Thread(() => setGame(client.GetClient()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void setGame(DiscordSocketClient client)
    {
        try
        {
            ActivityProperties activityProperties = new ActivityProperties();
            activityProperties.Type = ActivityType.Game;
            activityProperties.Name = textBox18.Text;
            client.SetActivity(activityProperties);
        }
        catch (Exception)
        {
        }
    }
    public void doSetOnlineStatus()
    {
        try
        {
            foreach (NovaClient client in novaClients)
            {
                try
                {
                    Thread.Sleep(trackBar10.Value);
                    Thread thread = new Thread(() => setOnlineStatus(client.GetClient()));
                    thread.Priority = ThreadPriority.Highest;
                    thread.Start();
                }
                catch (Exception)
                {
                }
            }
        }
        catch (Exception)
        {
        }
    }
    public void setOnlineStatus(DiscordSocketClient client)
    {
        try
        {
            if (comboBox1.SelectedIndex == 0)
            {
                client.SetStatus(UserStatus.Online);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                client.SetStatus(UserStatus.Idle);
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                client.SetStatus(UserStatus.DoNotDisturb);
            }
            else
            {
                client.SetStatus(UserStatus.Invisible);
            }
        }
        catch (Exception)
        {
        }
    }
 
}