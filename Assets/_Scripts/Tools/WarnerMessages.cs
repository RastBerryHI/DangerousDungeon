using UnityEngine;

public class WarnerMessages
{
    public static void UnassingedField(string field) => Debug.LogError($"Unassigned {field}");
    public static void NoComponent(string comp) =>  Debug.LogError($"No {comp}");
    public static void NoComponent(string comp, Object owner) =>  Debug.LogError($"No {comp} on {owner}");
    public static void NoComponentInParent(string comp, Object owner) => Debug.LogError($"No {comp} in parent of {owner}");
    public static void NoComponentInChildren(string comp, Object owner) => Debug.LogError($"No {comp} in children of {owner}");
}

