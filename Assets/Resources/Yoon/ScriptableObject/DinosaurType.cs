using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DinosaurType", menuName = "Scriptable Objects/DinosaurType")]
public class DinosaurType : ScriptableObject
{
    public string dinosaur_name;
    public List<string> dinosaur_type;
}
