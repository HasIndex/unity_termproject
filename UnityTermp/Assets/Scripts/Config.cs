using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ServerConfig
{
    public string serverIP;
    public int serverPort;
}


public static class Config
{
    public static string ServerIP { get; set; } = "127.0.0.1";
    public static int ServerPort { get; set; } = 21302;


}
