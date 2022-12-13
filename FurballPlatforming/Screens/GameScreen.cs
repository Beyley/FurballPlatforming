using Furball.Engine.Engine;
using FurballPlatforming.Drawables;

namespace FurballPlatforming.Screens;

public class GameScreen : Screen {
	public override void Initialize() {
		base.Initialize();
		
		this.Manager.Add(new GameSceneDrawable());
	}
}
