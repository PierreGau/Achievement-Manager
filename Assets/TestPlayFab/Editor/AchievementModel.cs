using UnityEngine;
//using System.Collections;
using UnityEditor;
using System.IO;

namespace Achievements
{
    public class AchievementModel : EditorWindow
    {
        [MenuItem("Achievements/Change Model")]
        static void OnOpenWindow()
        {
            AchievementModel tm = EditorWindow.GetWindow<AchievementModel>(true, "Achievement Model");
            tm.Show();
        }

        private void OnGUI()
        {

        }
    }
}

