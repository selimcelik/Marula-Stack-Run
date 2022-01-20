using UnityEngine;

[CreateAssetMenu(menuName = "ManagerFramework/Create Level", fileName = "Level_1")]
public class Level : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject prefab_Level;

    [Header("Background Color")]
    public Color color_Background1;
    public Color color_Background2;

    public LevelActor InitiliazeLevel()
    {
        // Spawn
        GameObject levelObj = (GameObject)Instantiate(prefab_Level, Vector3.zero, Quaternion.identity);

        return levelObj.GetComponent<LevelActor>();
    }
}
