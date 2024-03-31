using System.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;

namespace PumpFrame.Editor
{
    public class AutoAddressableGroups : ScriptableObject
    {
        static AddressableAssetSettings Settings
        {
            get { return AddressableAssetSettingsDefaultObject.Settings; }
        }

        static List<AddressableAssetGroupSchema> schemas;
        static AutoAddressableGroups()
        {
            schemas = new List<AddressableAssetGroupSchema>();
            schemas.Add(new ContentUpdateGroupSchema());
            schemas.Add(new BundledAssetGroupSchema());
        }

        [MenuItem("Pump框架/资源/Auto Addressable Groups")]
        static void ResetGroups()
        {
            string res_path = "Assets/_Res";
            DirectoryInfo res_folder = new DirectoryInfo(res_path);
            DirectoryInfo[] dics = res_folder.GetDirectories();

            foreach (var info in dics)
            {
                ResetGroup<UnityEngine.Object>(info.Name, $"{res_path}/{info.Name}", "t:object", assetPath =>
                {
                    string fileName = Path.GetFileNameWithoutExtension(assetPath);
                    string dirPath = Path.GetDirectoryName(assetPath);
                    string dirName = Path.GetFileNameWithoutExtension(dirPath);
                    //return $"{dirName}/{fileName}";
                    return $"{fileName}";
                });
            }
        }

        static void ResetGroup<T>(string groupName, string assetFolder, string filter, Func<string, string> getAddress)
        {
            string[] assets = GetAssets(assetFolder, filter);
            AddressableAssetGroup group = CreateGroup<T>(groupName);
            foreach (var assetPath in assets)
            {
                string address = getAddress(assetPath);
                AddAssetEntry(group, assetPath, address);
            }

            Debug.Log($"Reset group finished, group: {groupName}, asset folder: {assetFolder}, filter: {filter}, count: {assets.Length}");
        }

        static AddressableAssetGroup CreateGroup<T>(string groupName)
        {
            AddressableAssetGroup group = Settings.FindGroup(groupName);
            if (group == null)
                group = Settings.CreateGroup(groupName, false, false, false, schemas, typeof(T));   //null
            Settings.AddLabel(groupName, false);
            return group;
        }

        static AddressableAssetEntry AddAssetEntry(AddressableAssetGroup group, string assetPath, string address)
        {
            string guid = AssetDatabase.AssetPathToGUID(assetPath);

            AddressableAssetEntry entry = group.entries.FirstOrDefault(e => e.guid == guid);
            if (entry == null)
            {
                entry = Settings.CreateOrMoveEntry(guid, group, false, false);
            }

            entry.address = address;
            entry.SetLabel(group.Name, true, false, false);
            return entry;
        }

        public static string[] GetAssets(string folder, string filter)
        {
            folder = folder.TrimEnd('/').TrimEnd('\\');

            if (filter.StartsWith("t:"))
            {
                string[] guids = AssetDatabase.FindAssets(filter, new string[] { folder });
                string[] paths = new string[guids.Length];
                for (int i = 0; i < guids.Length; i++)
                    paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
                return paths;
            }
            else
            {
                throw new InvalidOperationException("Unexpected filter: " + filter);
            }
        }
    }
}