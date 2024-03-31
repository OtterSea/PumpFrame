using UnityEngine;

namespace PumpFrame
{
    public class CameraMgr : Singleton<CameraMgr>
    {
        private Transform _cameraTran;

        public static Transform CameraTran
        {
            get
            {
                if (Instance._cameraTran == null)
                {
                    Instance._cameraTran = Camera.main.transform;
                }
                return Instance._cameraTran;
            }
        }
    }
}