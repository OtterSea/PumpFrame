using UnityEngine;
using System;
using System.IO;
using UnityEngine.Assertions;
using System.Collections;

namespace PumpFrame.Editor
{
    public class IOEx
    {

        static public bool isFile(string path)
        {
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(path);
            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                return false;
            return true;
        }

        static public bool ExistFile(string path)
        {
            return File.Exists(path);
        }

        static public void CreateFile(string path)
        {
            File.Create(path).Dispose();
        }

        static public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        static public string GetFileNameWithoutExtension(string path)
        {
            string fn = Path.GetFileName(path);
            string ext = Path.GetExtension(path);
            return string.IsNullOrEmpty(ext)
                ? fn
                : fn.Replace(ext, "");
        }

        static public string GetPathWithoutExtension(string path)
        {
            string ext = Path.GetExtension(path);
            return string.IsNullOrEmpty(ext)
                ? path
                : path.Replace(ext, "");
        }

        static public string ReadAllText(string path)
        {
            var withOutBoomUtf8 = new System.Text.UTF8Encoding(false);
            return File.ReadAllText(path, withOutBoomUtf8);
        }

        static public string ReadAllTextFromStreamingAssets(string path)
        {
            path = String.Format("{0}/{1}", Application.streamingAssetsPath, path);

            string text;
            if (Application.platform == RuntimePlatform.Android)
            {
                var file = UnityEngine.Networking.UnityWebRequest.Get(path);
                file.SendWebRequest();
                while (!file.isDone)
                {
                }

                var result = file.result;
                if (result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                    result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(String.Format("ReadAllTextFromStreamingAssets Failed {0}", file.error));
                    text = null;
                }
                else
                {
                    text = file.downloadHandler.text;
                }
            }
            else
            {
                if (IOEx.ExistFile(path))
                {
                    text = IOEx.ReadAllText(path);
                }
                else
                {
                    text = null;
                }
            }

            return text;
        }

        static public void WriteAllText(string path, string str)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var withOutBoomUtf8 = new System.Text.UTF8Encoding(false);
            File.WriteAllText(path, str, withOutBoomUtf8);
        }

        static public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        static public byte[] ReadAllBytesFromStreamingAssets(string path)
        {
            path = String.Format("{0}/{1}", Application.streamingAssetsPath, path);
            byte[] bytes;
            if (Application.platform == RuntimePlatform.Android)
            {
                var file = UnityEngine.Networking.UnityWebRequest.Get(path);
                file.SendWebRequest();
                while (!file.isDone)
                {
                }

                var result = file.result;
                if (result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                    result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(String.Format("ReadAllBytesFromStreamingAssets Failed {0} error{1}", path,
                        file.error));
                    bytes = null;
                }
                else
                {
                    bytes = file.downloadHandler.data;
                }
            }
            else
            {
                if (IOEx.ExistFile(path))
                {
                    bytes = IOEx.ReadAllBytes(path);
                }
                else
                {
                    bytes = null;
                }
            }

            return bytes;
        }

        static public void WriteAllBytes(string path, byte[] bytes)
        {
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllBytes(path, bytes);
        }

        static public void AppendAllText(string path, string text)
        {
            File.AppendAllText(path, text);
        }

        static public bool ExistDir(string path)
        {
            return Directory.Exists(path);
        }

        static public void CreateDir(string path)
        {
            Directory.CreateDirectory(path);
        }

        static public string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        static public string GetDirName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        static public void FileMove(string from, string to)
        {
            var dir = Path.GetDirectoryName(to);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.Move(from, to);
        }

        static public void FileCopy(string from, string to, bool overwrite)
        {
            var dir = Path.GetDirectoryName(to);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.Copy(from, to, overwrite);
        }

        static public void DirCopy(string sourceDirectory, string targetDirectory)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(sourceDirectory);
                //获取目录下（不包含子目录）的文件和子目录
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo) //判断是否文件夹
                    {
                        if (!Directory.Exists(targetDirectory + "\\" + i.Name))
                        {
                            //目标目录下不存在此文件夹即创建子文件夹
                            Directory.CreateDirectory(targetDirectory + "\\" + i.Name);
                        }

                        //递归调用复制子文件夹
                        DirCopy(i.FullName, targetDirectory + "\\" + i.Name);
                    }
                    else
                    {
                        //不是文件夹即复制文件，true表示可以覆盖同名文件
                        File.Copy(i.FullName, targetDirectory + "\\" + i.Name, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("复制文件出现异常" + ex.Message);
            }
        }

        static public void DirDelete(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }

        static public void FileDelete(string path)
        {
            File.Delete(path);
        }

        static public string[] GetDirFiles(string dir, string searchPattern)
        {
            return Directory.GetFiles(dir, searchPattern, SearchOption.AllDirectories);
        }

        static public string[] GetDirDirs(string dir)
        {
            return Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);
        }

        static public string[] GetDirDirs(string dir, string searchPattern)
        {
            return Directory.GetDirectories(dir, searchPattern, SearchOption.AllDirectories);
        }

        static public bool isDirEmpty(string path)
        {
            if (!ExistDir(path)) return false;
            if (Directory.GetDirectories(path).Length > 0 || Directory.GetFiles(path).Length > 0)
            {
                return false;
            }

            return true;
        }

        static public void DeleteFileWithMeta(string path)
        {
            if (!File.Exists(path)) return;

            File.Delete(path);

            var meta = path + ".meta";

            if (File.Exists(meta))
                File.Delete(meta);
        }

        static public void DeleteDirectoryWithMeta(string path)
        {
            if (!Directory.Exists(path)) return;

            Directory.Delete(path, false);

            var meta = path + ".meta";

            if (File.Exists(meta))
                File.Delete(meta);

            path = IOEx.GetDirName(path);
            if (IOEx.isDirEmpty(path))
            {
                DeleteDirectoryWithMeta(path);
            }
        }
    }
}