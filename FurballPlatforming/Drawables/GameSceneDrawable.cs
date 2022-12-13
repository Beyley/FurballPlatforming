using System.Numerics;
using Furball.Engine;
using Furball.Engine.Engine.Graphics;
using Furball.Engine.Engine.Graphics.Drawables;
using Furball.Engine.Engine.Graphics.Drawables.Managers;
using Furball.Vixie;
using Furball.Vixie.Backends.Shared;
using FurballPlatforming.Collision;
using FurballPlatforming.Drawables.Entities;

namespace FurballPlatforming.Drawables; 

public class GameSceneDrawable : CompositeDrawable {
	public Collidable[] Collidables = Array.Empty<Collidable>();

	private Texture _collisionArrow;
	
	public GameSceneDrawable() {
		this.Children.Add(new PlayerEntityDrawable(this));

		this.Collidables = new[] {
			new Collidable(CollidableType.Floor, new Vector2(100, 100), 400),
			new Collidable(CollidableType.Wall, new Vector2(100, 150), 50, -MathF.PI / 2f)
		};

		this._collisionArrow = ContentManager.LoadTextureFromFileCached("arrow.png");
	}

	public override void Draw(double time, DrawableBatch batch, DrawableManagerArgs args) {
		//Draw all collidables
		foreach (Collidable collidable in this.Collidables) {
			Color col = collidable.Type switch {
				CollidableType.Wall    => Color.Blue,
				CollidableType.Floor   => Color.Green,
				CollidableType.Ceiling => Color.Orange,
				_                      => throw new ArgumentOutOfRangeException()
			};

			col.Af = 0.5f;

			batch.Draw(this._collisionArrow, collidable.Position, new Vector2(0.5f), collidable.Rotation, col);
			batch.Draw(FurballGame.WhitePixel, collidable.Position, new Vector2(collidable.Length, 1), collidable.Rotation, col);
		}

		base.Draw(time, batch, args);
	}
}
