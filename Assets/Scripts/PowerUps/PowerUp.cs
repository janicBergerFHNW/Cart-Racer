using System;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.UIElements;

namespace PowerUps
{
    public abstract class PowerUp : MonoBehaviour
    {
        public Sprite Icon;
        [NonSerialized] public Cart User;

        public abstract void OnUse(Cart user);
    }
}