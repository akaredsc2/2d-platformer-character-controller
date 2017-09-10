using UnityEngine;

public class AutoMoveCrate : PhysicsObject {
	private void Update () {
		targetVelocity = Vector2.left;
	}
}
