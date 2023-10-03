using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace PowerUps
{
    public class SummonPowerUp : PowerUp
    {

        [SerializeField] private Summon objectPrefab;

        public override void OnUse(Cart user)
        {
            var summon = Instantiate(objectPrefab, user.itemSpawn);
            summon.User = user;
        }

    }
}
