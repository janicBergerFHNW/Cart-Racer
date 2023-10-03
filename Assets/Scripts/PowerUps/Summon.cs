using System;
using UnityEngine;

namespace PowerUps
{
    public abstract class Summon : MonoBehaviour
    {
        [NonSerialized] public Cart User;
    }
}