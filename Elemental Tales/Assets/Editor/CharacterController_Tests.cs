using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController_Tests
{
    [Test]
    public void CharacterController_MoveFiveUnitsRight_Test()
    {
        CharacterControllerRaycast controller = new CharacterControllerRaycast();
        var input = new Vector2(1, 0);
        var moveAmount = new Vector2(5, 0);
        var expectedMovement = new Vector2(5, 0);

        var actualMovement = controller.Move(moveAmount, input);

        Assert.That(actualMovement, Is.EqualTo(expectedMovement));
    }
}
