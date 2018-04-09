using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;

public class TSCLIB_DLL
{
    [DllImport("TSCLIB.dll", EntryPoint = "about")]
    public static extern int about();

    [DllImport("TSCLIB.dll", EntryPoint = "openport")]
    public static extern int openport(string printername);

    [DllImport("TSCLIB.dll", EntryPoint = "barcode")]
    public static extern int barcode(string x, string y, string type,
                string height, string readable, string rotation,
                string narrow, string wide, string code);

    [DllImport("TSCLIB.dll", EntryPoint = "clearbuffer")]
    public static extern int clearbuffer();

    [DllImport("TSCLIB.dll", EntryPoint = "closeport")]
    public static extern int closeport();

    [DllImport("TSCLIB.dll", EntryPoint = "downloadpcx")]
    public static extern int downloadpcx(string filename, string image_name);

    [DllImport("TSCLIB.dll", EntryPoint = "formfeed")]
    public static extern int formfeed();

    [DllImport("TSCLIB.dll", EntryPoint = "nobackfeed")]
    public static extern int nobackfeed();

    [DllImport("TSCLIB.dll", EntryPoint = "printerfont")]
    public static extern int printerfont(string x, string y, string fonttype,
                    string rotation, string xmul, string ymul,
                    string text);

    [DllImport("TSCLIB.dll", EntryPoint = "printlabel")]
    public static extern int printlabel(string set, string copy);

    [DllImport("TSCLIB.dll", EntryPoint = "sendcommand")]
    public static extern int sendcommand(string printercommand);

    [DllImport("TSCLIB.dll", EntryPoint = "setup")]
    public static extern int setup(string width, string height,
              string speed, string density,
              string sensor, string vertical,
              string offset);

    [DllImport("TSCLIB.dll", EntryPoint = "windowsfont")]
    public static extern int windowsfont(int x, int y, int fontheight,
                    int rotation, int fontstyle, int fontunderline,
                    string szFaceName, string content);

}

namespace ZTSCPrint
{
    class Program
    {
        static void Main(string[] args)
        {

            System.Console.WriteLine("TSC Printing Program");
            System.Console.WriteLine("By Carlos");
            System.Console.WriteLine("Command Example : ZTSCPrint.exe carlos.conf test.txt");
            System.Console.WriteLine("Start to read configuration file...");

            String conf_file = null;
            String cmd_file = null;

            if (args.Length == 0)
                conf_file = "c:\\temp\\zhacar01.xml";
            else
                conf_file = args[0];


            XmlDocument d = new XmlDocument();
            try { 
            d.Load(conf_file);

                String conf_printer = "";
                String conf_width = "";
                String conf_height = "";
                String conf_speed = "";
                String conf_density = "";
                String conf_sensor = "";
                String conf_vertical = "";
                String conf_offset = "";

                foreach (XmlAttribute attr in d.DocumentElement.Attributes)
            {
                  
                    switch(attr.LocalName)
                    {
                        case "printer":
                            conf_printer = attr.Value;
                            break;
                        case "width":
                            conf_width = attr.Value;
                            break;
                        case "height":
                            conf_height = attr.Value;
                            break;
                        case "speed":
                            conf_speed = attr.Value;
                            break;
                        case "density":
                            conf_density = attr.Value;
                            break;
                        case "sensor":
                            conf_sensor = attr.Value;
                            break;
                        case "vertical":
                            conf_vertical = attr.Value;
                            break;
                        case "offset":
                            conf_offset = attr.Value;
                            break;
                    }
            }

                if (args.Length <= 1)
                    cmd_file = "c:\\temp\\zhacar01.txt";
                else
                    cmd_file = args[1];

                System.Console.WriteLine("Start to read command file...");
                StreamReader streamReader = new StreamReader(cmd_file);
                string cmd_text = streamReader.ReadToEnd();
                streamReader.Close();

                System.Console.WriteLine("Start to setup printer...");
                TSCLIB_DLL.openport(conf_printer);
                TSCLIB_DLL.setup(conf_width, conf_height, conf_speed, conf_density, conf_sensor, conf_vertical, conf_offset);
                TSCLIB_DLL.clearbuffer();
                System.Console.WriteLine("Start to send command to printer...");
                TSCLIB_DLL.sendcommand(cmd_text);
                TSCLIB_DLL.closeport();
                System.Console.WriteLine("Done !");
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error :" + e.Message);
            }
        }
    }
}
