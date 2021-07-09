using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.IO;
namespace TscanCodeFilter
{
    public partial class TscanCodeFilter : Form
    {
        private FileStream m_LogFile = null;
        private StreamWriter m_LogSW = null;
        private string m_FilePath = "";
        private uint m_VersionId = 0;
        private XmlDocument m_LoadXmlDoc = new XmlDocument();
        public class FilterNode
        {
            public string line;
            public string severity;
            public bool isFilter = false;
        }
        private Dictionary<string, List<FilterNode>> m_FilterNodeDic = new Dictionary<string, List<FilterNode>>();
        private List<string> m_Keys = null;
        public TscanCodeFilter()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            if (m_LogSW != null)
            {
                m_LogSW.Close();
                m_LogSW = null;
            }
            if (m_LogFile != null)
            {
                m_LogFile.Close();
                m_LogFile = null;
            }
            if (m_Keys != null)
            {
                m_Keys.Clear();
            }
            if (m_FilterNodeDic != null)
            {
                m_FilterNodeDic.Clear();
            }
            if (File.Exists("Log.txt"))
            {
                File.Delete("Log.txt");
            }
            if (Directory.Exists("Tmp"))
            {
                Directory.Delete("Tmp", true);
            }
            m_LogFile = new FileStream("Log.txt", FileMode.OpenOrCreate);
            m_LogSW = new StreamWriter(m_LogFile);
            string path = string.Empty;
            var openFileDialog = new OpenFileDialog()
            {
                Filter = "Files (*.xml)|*.xml"//如果需要筛选txt文件（"Files (*.txt)|*.txt"）
            };
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = openFileDialog.FileName;
                showxml.Text = path;
                m_FilePath = path;
                m_LoadXmlDoc.Load(m_FilePath);
                LoadXmlData();
            }
        }
        private void LoadXmlData()
        {
            XmlElement element = m_LoadXmlDoc.DocumentElement;
            XmlNodeList nodes = element.ChildNodes;
            foreach (var node in nodes)
            {
                XmlElement child = (XmlElement)node;
                string file = child.GetAttribute("file").ToString();
                FilterNode fn = new FilterNode();
                fn.line = child.GetAttribute("line").ToString();
                fn.severity = child.GetAttribute("severity").ToString();
                if (!m_FilterNodeDic.ContainsKey(file))
                {
                    List<FilterNode> nodeList = new List<FilterNode>();
                    nodeList.Add(fn);
                    m_FilterNodeDic.Add(file, nodeList);
                }
                else
                {
                    m_FilterNodeDic[file].Add(fn);
                }
                int pos = file.LastIndexOf("\\");
                string fileName = file.Substring(pos + 1);
                exportxml.Text = string.Format("加载xml数据文件：{0}！", fileName);
                m_LogSW.WriteLine(exportxml.Text);
            }
            exportxml.Text = string.Format("加载完成！加载xml数据{0}行！涉及到的代码文件{1}个！", nodes.Count, m_FilterNodeDic.Count);
            m_LogSW.WriteLine(exportxml.Text);
            //m_LogSW.Close();
        }

        private async void filterBtn_Click(object sender, EventArgs e)
        {
            this.filterBtn.Enabled = false;
            if (!Directory.Exists("Tmp"))
            {
                Directory.CreateDirectory("Tmp");
            }
            string versionStr = versionIdTB.Text;
            if (!uint.TryParse(versionStr, out m_VersionId))
            {
                MessageBox.Show("文本框需要输入一个版本号，正整数！");
                return;
            }
            try
            {
                m_Keys = new List<string>(m_FilterNodeDic.Keys);

                Process m_Process = new Process();
                m_Process.StartInfo.FileName = "cmd.exe";  //设置要启动的应用程序
                m_Process.StartInfo.UseShellExecute = false;  //是否使用操作系统shell启动
                m_Process.StartInfo.RedirectStandardInput = true;  // 接受来自调用程序的输入信息
                m_Process.StartInfo.RedirectStandardOutput = false;  //输出信息
                m_Process.StartInfo.RedirectStandardError = true;  // 输出错误
                m_Process.StartInfo.CreateNoWindow = true;  //不显示程序窗口
                m_Process.Start();  //启动程序
                string command = "";
                List<string> outPutList = new List<string>();
                for (int i = 0; i < m_Keys.Count; ++i)
                {
                    command = GetWriteCommand(m_Keys[i]);
                    await m_Process.StandardInput.WriteLineAsync(command);
                    string name = GetFileNameFromAllName(m_Keys[i]);
                    exportxml.Text = string.Format("{0}:生成临时文件{1}！", i, name);
                    await m_Process.StandardInput.FlushAsync();
                }
                AnalysisFilter();
                FilterResult();
                this.filterBtn.Enabled = true;
            }
            catch (Exception exception)
            {
                //m_LogSW = new StreamWriter(m_LogFile);
                m_LogSW.WriteLine(exception.Message);
            }
        }
        private string GetWriteCommand(string srcFile)
        {
            string file = srcFile;
            int pos = file.LastIndexOf("\\");
            string fileName = file.Substring(pos + 1);
            string[] fileNameSplit = fileName.Split('.');
            string command = string.Format("svn blame {0} --xml>>Tmp/{1}.xml", srcFile, fileNameSplit[0]+ fileNameSplit[1]);
            //string command = string.Format("svn blame {0}>>Tmp/{1}.xml", srcFile, fileNameSplit[0] + fileNameSplit[1]);
            return command;
        }
        private string GetFileNameFromAllName(string srcFile)
        {
            string file = srcFile;
            int pos = file.LastIndexOf("\\");
            string fileName = file.Substring(pos + 1);
            return fileName;
        }

        private void AnalysisFilter()
        {
            try
            {
                for (int i = 0; i < m_FilterNodeDic.Count; ++i)
                {
                    string file = m_Keys[i];
                    int pos = file.LastIndexOf("\\");
                    string fileName = file.Substring(pos + 1);
                    string[] fileNameSplit = fileName.Split('.');
                    string xmlName = string.Format("Tmp/{0}.xml", fileNameSplit[0] + fileNameSplit[1]);
                    List<FilterNode> list = m_FilterNodeDic[file];
                    XmlDocument document = new XmlDocument();
                    document.Load(xmlName);
                    for (int j = 0; j < list.Count; ++j)
                    {
                        FilterNode fn = list[j];

                        if (IsFilter(document, xmlName, fn.line))
                        {
                            fn.isFilter = true;
                        }
                    }
                    exportxml.Text = string.Format("{0}：分析{1}完成！", i, xmlName);
                    m_LogSW.WriteLine(exportxml.Text);
                }
                exportxml.Text = string.Format("分析完成！分析xml文件{0}个！", m_FilterNodeDic.Count);
                m_LogSW.WriteLine(exportxml.Text);
            }
            catch(Exception e)
            {
                m_LogSW.WriteLine(e.Message);
            }
        }
        
        private bool IsFilter(XmlDocument document, string xmlName, string line)
        {
            XmlNodeList nodes = document.SelectNodes("blame/target/entry");
            for (int i = 0; i < nodes.Count; ++i)
            {
                XmlElement child = (XmlElement)nodes[i];
                string tmpLine = child.GetAttribute("line-number").ToString();
                XmlElement node = (XmlElement)child.SelectSingleNode("commit");
                if ( node == null )continue;
                string revision = node.GetAttribute("revision").ToString();
                uint revisionId = 0;
                if (tmpLine == line && uint.TryParse(revision, out revisionId) )
                {
                    if (revisionId > m_VersionId)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void FilterResult()
        {
            XmlDocument document = new XmlDocument();
            document.Load(m_FilePath);
            XmlElement element = document.DocumentElement;
            XmlNodeList nodes = element.ChildNodes;
            List<XmlNode> delNodeList = new List<XmlNode>();
            for (int i = 0; i < nodes.Count; ++i)
            {
                XmlElement child = (XmlElement)nodes[i];
                string file = child.GetAttribute("file").ToString();
                string line = child.GetAttribute("line").ToString();
                string severity = child.GetAttribute("severity").ToString();
                List<FilterNode> list = m_FilterNodeDic[file];
                for (int j = 0; j < list.Count; ++j)
                {
                    if (list[j].isFilter && list[j].line == line)
                    {
                        delNodeList.Add(nodes[i]);
                        break;
                    }
                }
            }
            foreach (XmlNode node in delNodeList)
            {
                element.RemoveChild(node);
            }
            int pos = m_FilePath.LastIndexOf("\\");
            string fileName = m_FilePath.Substring(pos + 1);
            string finalName = string.Format("Result_{0}", fileName);
            document.Save(finalName);
            exportxml.Text = string.Format("导出{0}完成!", finalName);
            m_LogSW.WriteLine(exportxml.Text);
        }

        private void TscanCodeFilter_Close(object sender, FormClosedEventArgs e)
        {
            if (m_LogSW != null)
            {
                m_LogSW.Close();
            }
        }
    }
}
