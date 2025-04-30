using UnityEngine;
using NUnit.Framework;
using UnityEngine;

public class PlayerEditModeTests
{
    [Test]
        public void VerifyPlayerHealthAtStartPasses()
        {
            var GameObject = new GameObject();

            var Player = GameObject.AddComponent<Player>();

            Assert.AreEqual(3, Player.maxHearts);

            //yield return null;
           
        }
    
}
