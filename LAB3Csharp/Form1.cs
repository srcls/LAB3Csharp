using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace LAB3Csharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
   
                Regex regex = new Regex(@"""([^""]+\.csproj)""");

                var SLN_FILES = Directory.GetFiles(fbd.SelectedPath, "*.sln", SearchOption.AllDirectories);
                
                List<string> CSPROJ_FILES = new List<string>();
                
                foreach (var SLN_FILE in SLN_FILES)
                {
                    var file = File.ReadAllText(SLN_FILE);

                    MatchCollection matches = regex.Matches(file);
                    foreach (Match match in matches)
                    {
                        CSPROJ_FILES.Add(match.Groups[1].ToString());
                    }
                }

                foreach (var CSPROJ_FILE in CSPROJ_FILES)
                {

                    XmlDocument csproj = new XmlDocument();
                    string dir = fbd.SelectedPath + "\\" + CSPROJ_FILE;


                    csproj.Load(dir);
                    string subfolder = Regex.Replace(CSPROJ_FILE, @"\\.+\.csproj", "");

                    string dir2 = fbd.SelectedPath + "\\" + subfolder + "\\";

                    XDocument xmldoc = XDocument.Load(dir);
                    XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

                    DirectoryInfo di = Directory.CreateDirectory(fbd.SelectedPath + "\\" +"kopia" + "\\" + subfolder + "\\");

                    foreach (var resource in xmldoc.Descendants(msbuild + "Compile"))
                    {
                        string includePath = resource.Attribute("Include").Value;
                        string subsubfolder = Regex.Replace(includePath, @"\\[^\\]+$", "");
                        string finaldir;
                        if (subsubfolder == includePath)
                            finaldir = fbd.SelectedPath + "\\" + "kopia" + "\\" + subfolder + "\\";
                        else
                            finaldir = fbd.SelectedPath + "\\" + "kopia" + "\\"  + subfolder + "\\" + subsubfolder + "\\";
                        Directory.CreateDirectory(finaldir);
                        File.Copy(dir2 + includePath, fbd.SelectedPath + "\\" + "kopia" + "\\"  + subfolder + "\\" + includePath, true);


                    }
                    foreach (var resource in xmldoc.Descendants(msbuild + "EmbeddedResource"))
                    {
                        string includePath = resource.Attribute("Include").Value;
                        string subsubfolder = Regex.Replace(includePath, @"\\[^\\]+$", "");
                        string finaldir;
                        if (subsubfolder == includePath)
                            finaldir = fbd.SelectedPath + "\\" + "kopia" + "\\" + subfolder + "\\";
                        else
                            finaldir = fbd.SelectedPath + "\\" + "kopia" + "\\" + subfolder + "\\" + subsubfolder + "\\";
                        Directory.CreateDirectory(finaldir);
                        File.Copy(dir2 + includePath, fbd.SelectedPath + "\\" + "kopia" + "\\" + subfolder + "\\" + includePath, true);
                    }
                    foreach (var resource in xmldoc.Descendants(msbuild + "None"))
                    {
                        string includePath = resource.Attribute("Include").Value;
                        string subsubfolder = Regex.Replace(includePath, @"\\[^\\]+$", "");
                        string finaldir;
                        if (subsubfolder == includePath)
                            finaldir = fbd.SelectedPath + "\\" + "kopia" + "\\" + subfolder + "\\";
                        else
                            finaldir = fbd.SelectedPath + "\\" + "kopia" + "\\" + subfolder + "\\" + subsubfolder + "\\";
                        Directory.CreateDirectory(finaldir);
                        File.Copy(dir2 + includePath, fbd.SelectedPath + "\\" + "kopia" + "\\" + subfolder + "\\" + includePath, true);
                    }
                }

            }
        }
    }
}
