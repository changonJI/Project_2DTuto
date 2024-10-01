using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public static class MenuItemUtil
{
    private const string Ctrl = "%";
    private const string Alt = "&";
    private const string Shift = "#";

    #region 경로 : Util/
    /// <summary>
    /// 선택한 오브젝트 on/off
    /// </summary>
    [MenuItem("Util/ObjectOnOff " + Alt + "a", false, 0)]
    private static void ObjectOnOff()
    {
        Selection.activeGameObject.SetActive(!Selection.activeGameObject.activeSelf);
    }

    /// <summary>
    /// LogoScene Open
    /// </summary>
    [MenuItem("Util/OpenLogoScene " + Shift + Alt + "1", false, 10)]
    private static void OpenLogoScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/LoadingScene.unity");
    }

    /// <summary>
    /// TitleScene Open
    /// </summary>
    [MenuItem("Util/OpenTitleScene " + Shift + Alt + "2", false, 11)]
    private static void OpenTitleScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TitleScene.unity");
    }

    /// <summary>
    /// TownScene Open
    /// </summary>
    [MenuItem("Util/OpenTownScene " + Shift + Alt + "3", false, 12)]
    private static void OpenTownScene()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/TownScene.unity");
    }
    #endregion

    #region 경로 : Assets/
    /// <summary>
    /// AssetBundles Build
    /// </summary>
    [MenuItem("Assets/Asset Bundles Build")]
    private static void BuildAllAssetBundles()
    {
        string assetBundleDir = Application.dataPath + "/" + "AssetBundle";

        if (!Directory.Exists(assetBundleDir))
        {
            Directory.CreateDirectory(assetBundleDir);
        }

        // (저장 경로,빌드 옵션, 현재 빌드 세팅한 OS
        BuildPipeline.BuildAssetBundles(assetBundleDir, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
    #endregion
}
