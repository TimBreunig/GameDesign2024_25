using UnityEngine;

[CreateAssetMenu(fileName = "SceneTracker", menuName = "Game/SceneTracker", order = 1)]
public class SceneTracker : ScriptableObject
{
    public bool hasSceneBeenPlayed = false;
}