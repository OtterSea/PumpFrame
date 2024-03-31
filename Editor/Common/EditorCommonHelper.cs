using UnityEditor;
using UnityEngine;
using System.IO;

namespace PumpFrame.Editor
{
    public class EditorCommonHelper
    {
        /// <summary>
        /// 依据asset文件名和过滤选项找到对应的文件在Assets文件夹里的地址
        /// 使用实例：
        ///
        /// 获得名为 AnimStateValueDrawer 的 UXML 文件：
        /// string floderPath = EditorCommonHelper.GetAssetFloderPathByName("ListDrawer", "t:VisualTreeAsset");
        ///
        /// 返回值举例：Assets/_FrameWork/Editor/ScriptTemplates/ScriptCreatorWindow.cs
        /// 
        /// 后续使用举例，加载对应资产：
        /// var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(floderPath);
        /// 
        /// 其他的类型：
        /// t:Script
        /// t:StyleSheet
        ///
        /// 去除得到的路径 path 的文件 .cs后缀
        /// _path.Replace((@"/" + assetName + ".cs"), "/");
        ///
        /// 可以点击对应的资源，从 inspector 面板看到他的内部实例类，并作为 t: 的搜索参数
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string GetAssetFloderPathByName(string assetName, string filter = null, bool getFullPath = false)
        {
            string param = assetName;
            if (param != null)
            {
                param = $"{filter} {assetName}";
            }
            string[] path = UnityEditor.AssetDatabase.FindAssets(param);
            if (path.Length > 1)
            {
                Debug.LogError("存在多个此文件：" + assetName);
                return null;
            }
            if (path.Length <= 0)
            {
                Debug.LogError("不存在此文件：" + assetName);
                return null;
            }
            string _path = AssetDatabase.GUIDToAssetPath(path[0]);

            if (getFullPath)
            {
                return _path;
            }

            int subIndex = _path.IndexOf(assetName);
            _path = _path.Substring(0, subIndex);
            return _path;
        }

        /// <summary>
        /// searchPattern 的写法举例：
        ///  所有任何文件                  --  "*"
        ///  所有 .txt 结尾的文件          --  "*.txt"
        ///  多种指定类型的文件            --  "*.meta|*.xls|*.cs|*.png"
        ///  ABList_开头的任何.txt文件     --  "ABList_*.txt"
        ///
        /// FileInfo是微软提供的文件本身的描述数据，可以访问文件名等：
        /// info.Name
        /// info.FullName
        /// info.DirectoryName
        /// ...
        /// 
        /// </summary>
        /// <param name="path">查找的文件夹路径</param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static FileInfo[] GetAllFileInfo(string path, string searchPattern)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo direction = new DirectoryInfo(path);
                FileInfo[] files = direction.GetFiles(searchPattern);
                return files;
            }
            Debug.LogError("此路径不存在：" + path);
            return null;
        }
    }
}