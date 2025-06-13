using UnityEditor;
using System.IO;

public class AssetBundleBuilder
{
    [MenuItem("Tools/Build AssetBundle")]
    public static void BuildAssetBundle()
    {
        string path = "Assets/StreamingAssets";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
