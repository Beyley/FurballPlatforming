using System.Drawing;
using System.Numerics;
using Furball.Engine.Engine.Graphics;
using Furball.Engine.Engine.Graphics.Drawables;
using Furball.Engine.Engine.Graphics.Drawables.Managers;
using Furball.Engine.Engine.Helpers;
using Color = Furball.Vixie.Backends.Shared.Color;

namespace FurballPlatforming.Drawables; 

public abstract class GameEntityDrawable : CompositeDrawable {
	/// <summary>
	/// Whether or not to display the hitbox of the player
	/// </summary>
	public bool DisplayHitbox = false;

	/// <summary>
	/// The hitbox of the entity
	/// </summary>
	public Vector2 HitboxSize;
	
	/// <summary>
	/// The associated scene with this entity
	/// </summary>
	public readonly GameSceneDrawable Scene;

	public override Vector2 Size => this.HitboxSize;

	protected GameEntityDrawable(GameSceneDrawable scene) {
		this.DisplayHitbox = true;
		this.Scene         = scene;
	}

	public override void Draw(double time, DrawableBatch batch, DrawableManagerArgs args) {
		batch.DrawRectangle(
			args.Position,
			this.HitboxSize * args.Scale,
			1,
			this.Scene.Collidables.Any(x => x.Collides(this))
				? Color.Green
				: Color.Red
		);
	}
}
