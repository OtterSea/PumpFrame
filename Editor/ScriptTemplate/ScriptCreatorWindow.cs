using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using Sirenix.Utilities;

namespace PumpFrame.Editor
{
    public class ScriptCreatorWindow : EditorWindow
    {
        public enum TemplateType
        {
            CSharp,
        }

        private static string TemplatePath
        {
            get
            {
                string path = EditorCommonHelper.GetAssetFloderPathByName("ScriptCreatorWindow", "t:Script");
                return path;
            }
        }
        private TemplateType _type;
        private string _templateName;
        private string _scriptName;
        private string _nameSpace;
        private const string namespaceKey = "PumpFrame-Editor-ScriptCreator-NameSpace";

        private static int _selectFileIndex;
        private static string[] _allFileFullName;

        [MenuItem("Assets/Pump框架/Script Creator", false, 1)]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow(typeof(ScriptCreatorWindow));
            var screenResolution = Screen.currentResolution;
            var w = 300;
            var h = 150;
            var r = new Rect(screenResolution.width * 0.5f - w * 0.5f, screenResolution.height * 0.5f - h * 0.5f, w, h);
            window.position = r;

            //模板名加载
            var allFile = EditorCommonHelper.GetAllFileInfo(TemplatePath, "*.txt");
            _allFileFullName = new string[allFile.Length];
            var i = 0;
            allFile.ForEach(f =>
            {
                _allFileFullName[i++] = f.Name;
            });
        }

        //窗口绘制
        void OnGUI()
        {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);

            //模板类型
            _type = (TemplateType)EditorGUILayout.EnumPopup("TemplateType", _type);

            //模板名
            _selectFileIndex = EditorGUILayout.Popup("TemplateName", _selectFileIndex, _allFileFullName);
            _templateName = _allFileFullName[_selectFileIndex];

            //命名空间
            _nameSpace = EditorGUILayout.TextField("NameSpace", _nameSpace);
            if (string.IsNullOrEmpty(_nameSpace))
            {
                if (PlayerPrefs.HasKey(namespaceKey))
                {
                    _nameSpace = PlayerPrefs.GetString(namespaceKey);
                    if (string.IsNullOrEmpty(_nameSpace))
                    {
                        _nameSpace = null;
                    }
                }
                else
                {
                    _nameSpace = null;
                }
            }
            
            //文件名
            _scriptName = EditorGUILayout.TextField("ScriptName", _scriptName);
            if (string.IsNullOrEmpty(_scriptName))
            {
                _scriptName = "NewScript";
            }

            //创建按钮
            if (GUILayout.Button("Create"))
            {
                Create();
                Close();
            }
        }

        private void Create()
        {
            var templateSource = AssetDatabase.LoadAssetAtPath<TextAsset>($"{TemplatePath}{_templateName}");

            if (templateSource == null)
            {
                Debug.Log("no template file");
                return;
            }

            string codeSrc = templateSource.text;
            if (string.IsNullOrEmpty(codeSrc))
            {
                Debug.Log("no template content");
                return;
            }
            //模板文本替换处理
            codeSrc = DealCodeSrc(codeSrc);
            var assetPath = string.Format("{0}/{1}{2}", GetSelectedPathOrFallback(), _scriptName, ".cs");
            var filePath = Application.dataPath.Replace("Assets", assetPath);
            File.WriteAllText(filePath, codeSrc);
            AssetDatabase.ImportAsset(assetPath);
        }

        private string DealCodeSrc(string codeSrc)
        {
            switch (_type)
            {
                case TemplateType.CSharp:
                    //文件名
                    codeSrc = codeSrc.Replace("#ScriptName#", _scriptName);
                    //命名空间
                    if (_nameSpace != null)
                    {
                        codeSrc = codeSrc.Replace("#NameSpaceBegin#", $"namespace {_nameSpace}\n{{");
                        codeSrc = codeSrc.Replace("#NameSpaceEnd#", "}");
                        PlayerPrefs.SetString(namespaceKey, _nameSpace);
                    }
                    else
                    {
                        codeSrc = codeSrc.Replace("#NameSpaceBegin#", "");
                        codeSrc = codeSrc.Replace("#NameSpaceEnd#", "");
                    }
                    break;
            }
            return codeSrc;
        }

        public static string GetSelectedPathOrFallback()
        {
            string path = "Assets";

            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    //如果是目录获得目录名，如果是文件获得文件所在的目录名
                    path = Path.GetDirectoryName(path);
                    break;
                }
            }

            return path;
        }
    }
}