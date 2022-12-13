using System.Drawing;
using System.Numerics;
using Furball.Engine;

namespace FurballPlatforming.Drawables.Entities; 

public class PlayerEntityDrawable : GameEntityDrawable {
	public PlayerEntityDrawable(GameSceneDrawable scene) : base(scene) {
		this.HitboxSize = new Vector2(20, 40);

		FurballGame.InputManager.OnKeyDown += (sender, args) => {
			this.Position += new Vector2(10);
		};
	}
}
