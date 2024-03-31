using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 
/// </summary>

namespace PumpFrame
{
    [CreateAssetMenu(menuName = "PumpFrame/Create GameSetting", fileName = "PumpGameSetting")]
    public class GameSetting : SingletonScriptableObject<GameSetting>
    {
        public string gameName;
        public string gameVersion;

        #region 输入部分

        public InputActionAsset inputActionAsset;

        public static InputActionMap GetInputActionMapByName(string mapName)
        {
            if (Instance.inputActionAsset == null)
            {
                throw new System.Exception("没有inputActionAsset");
            }
            var map = Instance.inputActionAsset.FindActionMap(mapName);
            if (map == null)
            {
                throw new System.Exception($"inputActionAsset中没有找到对应的map，其名应为：{mapName}");
            }
            return map;
        }

        #endregion
    }
}
