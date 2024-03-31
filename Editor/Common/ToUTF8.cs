using System;
using System.IO;
using System.Text;
using UnityEditor;

//将整个工程文件夹下的所有C#脚本都变为UTF-8编码

namespace PumpFrame.Editor
{
    public static class ToUTF8
    {
        /// <summary>
        /// 把.cs转成UTF-8格式
        /// </summary>
        //[MenuItem("PumpZai/CommonTool/ConvertToUTF8")]
        public static void ConvertToUTF8()
        {
            var dir = "Assets/";        //0_Framework
            foreach (var f in new DirectoryInfo(dir).GetFiles("*.cs", SearchOption.AllDirectories))
            {
                var s = File.ReadAllText(f.FullName, Encoding.Default);
                try
                {
                    File.WriteAllText(f.FullName, s, Encoding.UTF8);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
    }
}