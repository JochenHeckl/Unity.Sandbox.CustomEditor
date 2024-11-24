using System;
using UnityEngine;

[Serializable]
public class NamedInt
{
    public string name;
    public int value;
}

[Serializable]
public class CustomArrayObject : MonoBehaviour
{
    public NamedInt[] namedInts = Array.Empty<NamedInt>();
}
