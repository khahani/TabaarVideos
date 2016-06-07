using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Helpers;
using System.Web.Script.Serialization;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            GenerateJson("http://www.aparat.com/etc/api/videobyuser/username/tabaar", "latest");
            //GenerateJson("http://www.aparat.com/etc/api/videobyprofilecat/usercat/13991/username/tabaar", "کلاس های آموزشی");
            CreateHtml("latest");
            Console.WriteLine("done.");
            Console.ReadKey();
        }

        private static void GenerateJson(string src, string filename)
        {
           string content = new System.Net.WebClient().DownloadString(src);

            StreamWriter jsonFile = File.CreateText(Application.StartupPath + "\\" + filename + ".json");

            jsonFile.Write(content);

            jsonFile.Close();
        }

        private static void CreateHtml(string filename)
        {
            string json = File.ReadAllText(Application.StartupPath + "\\" + filename + ".json");

            string defaultHtml = File.ReadAllText(Application.StartupPath + "\\base.html");

            List<string> UIDs = new List<string>();

            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic obj = serializer.Deserialize(json, typeof(object));

            string videosHtml = "";

            for (int i = 0; i < obj.videobyuser.Count; i++)
            {
                string uid = obj.videobyuser[i].uid;
                videosHtml += "<iframe src='http://www.aparat.com/video/video/embed/videohash/" + uid + "/vt/frame' allowFullScreen=\"true\" webkitallowfullscreen=\"true\" mozallowfullscreen=\"true\" ></iframe>";
            }
            
            defaultHtml = defaultHtml.Replace("$", videosHtml);

            StreamWriter htmlFile = File.CreateText(Application.StartupPath + "\\" + filename + ".html");

            htmlFile.Write(defaultHtml);

            htmlFile.Close();
        }

       
    }
}
