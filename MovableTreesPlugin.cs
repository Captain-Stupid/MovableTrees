using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MovableTrees;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class MovableTrees : BasePlugin
{

    public const string GUID = "com.CaptainStupid.StupidMod";
    public const string NAME = "StupidMod";
    public const string VERSION = "1.0.3";

    public static Manager manager;

    internal static Harmony harmony;

    internal static MovableTrees Instance { get; private set; }
    public static ManualLogSource Logger { get; private set; }


    public override void Load()
    {
        Instance = this;
        Logger = base.Log;

        harmony = new Harmony(GUID);
        
        //load resources
        var ourAssembly = Assembly.GetExecutingAssembly();
        harmony.PatchAll(typeof(MovableTrees));
        var resources = ourAssembly.GetManifestResourceNames();
        foreach (var resource in resources)
        {
            if (!resource.EndsWith(".png"))
                continue;

            var stream = ourAssembly.GetManifestResourceStream(resource);

            var ms = new MemoryStream();
            stream.CopyTo(ms);
            var resourceName = Regex.Match(resource, @"([a-zA-Z\d\-_]+)\.png").Groups[1].ToString();
            ResourceManager.LoadSprite("MovableTrees", resourceName, ms.ToArray());
        }
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }

    [HarmonyPatch(typeof(Manager), nameof(Manager.Awake))]
    [HarmonyPostfix]
    public static void PostManagerAwake()
    {
        Il2CppSystem.Collections.Generic.List<EntityMonoBehaviourData> prefabList = Manager._instance._ecsManager.ecsPrefabTable.prefabList[0].GetComponent<PugDatabaseAuthoring>().prefabList;

        LocalizationManager.AddTerm($"Items/{ObjectID.GraveTree}", "Grave Tree");
        LocalizationManager.AddTerm($"Items/{ObjectID.KelpTree}", "Kelp Tree");

        foreach (var prefab in prefabList)
        {
            if (prefab.name.ToLower().Contains("gravetree") || prefab.name.ToLower().Contains("kelptree") || prefab.name.ToLower().Contains("moldtree"))
            {
                prefab.gameObject.AddComponent<MineableCDAuthoring>();
                var health = prefab.gameObject.AddComponent<HealthCDAuthoring>();
                health.maxHealth = 3;
                health.startHealth = 3;
                prefab.gameObject.AddComponent<DeathStateCDAuthoring>();
                prefab.gameObject.AddComponent<TookDamageStateCDAuthoring>();
                prefab.gameObject.AddComponent<DeathStateCDAuthoring>();
                var pp = prefab.gameObject.AddComponent<PixelPerfectPosition2>();
                var damageReduction = prefab.gameObject.AddComponent<DamageReductionCDAuthoring>();
                damageReduction.maxDamagePerHit = 1;
                prefab.gameObject.AddComponent<DestructibleObjectCDAuthoring>();
                prefab.objectInfo.objectType = ObjectType.PlaceablePrefab;
                prefab.objectInfo.icon = ResourceManager.GetSprite($"MovableTrees.{prefab.name}16x");
                prefab.objectInfo.smallIcon = ResourceManager.GetSprite($"MovableTrees.{prefab.name}8x");
            }
        }
    }
}
