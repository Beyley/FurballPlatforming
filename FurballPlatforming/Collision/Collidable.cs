using System.Drawing;
using System.Numerics;
using FurballPlatforming.Drawables;
using RectangleF = Eto.Drawing.RectangleF;

namespace FurballPlatforming;

public enum CollidableType {
	Wall,
	Floor,
	Ceiling
}

public struct Collidable {
	public readonly CollidableType Type;
	public readonly Vector2        Position;
	public readonly float          Length;
	public readonly float          Rotation;

	private readonly float _sin;
	private readonly float _cos;

	private const float WIDTH = 5f;

	public Collidable(CollidableType type, Vector2 position, float length, float rotation = 0) {
		this.Type     = type;
		this.Position = position;
		this.Length   = length;
		this.Rotation = rotation;

		this._sin = MathF.Sin(this.Rotation);
		this._cos = MathF.Cos(this.Rotation);
	}

	public bool Collides(GameEntityDrawable entity) {
		//The 4 vertices of the rectangle
		(Vector2 rtl, Vector2 rtr, Vector2 rbl, Vector2 rbr) = (
			entity.Position,
			entity.Position + new Vector2(entity.HitboxSize.X, 0),
			entity.Position + new Vector2(0, entity.HitboxSize.Y),
			entity.Position + entity.HitboxSize
		);

		(Vector2 tl, Vector2 tr, Vector2 bl, Vector2 br) = (
			this.Position,
			new Vector2(this.Position.X                  + this.Length, this.Position.Y),
			new Vector2(this.Position.X, this.Position.Y + WIDTH),
			this.Position + new Vector2(this.Length, WIDTH)
		);

		tl = this.RotateVector2(tl);
		tr = this.RotateVector2(tr);
		bl = this.RotateVector2(bl);
		br = this.RotateVector2(br);

		Vector2[] points  = { rtl, rtr, rbl, rbr };
		Vector2[] points2 = { tl, tr, bl, br };

		if (this.DoAxisSeparationTest(tl, tr, bl, points))
			return false;

		if (this.DoAxisSeparationTest(tl, bl, br, points))
			return false;

		if (this.DoAxisSeparationTest(tr, br, tl, points))
			return false;

		if (this.DoAxisSeparationTest(bl, br, tl, points))
			return false;

		if (this.DoAxisSeparationTest(rtl, rtr, rbl, points2))
			return false;

		if (this.DoAxisSeparationTest(rtl, rbl, rbr, points2))
			return false;

		if (this.DoAxisSeparationTest(rtr, rbr, rtl, points2))
			return false;

		if (this.DoAxisSeparationTest(rbl, rbr, rtl, points2))
			return false;	
		
		return true;
	}

	/// <summary>
	/// Does axis separation test for a convex quadrilateral.
	/// </summary>
	/// <param name="x1">Defines together with x2 the edge of quad1 to be checked whether its a separating axis.</param>
	/// <param name="x2">Defines together with x1 the edge of quad1 to be checked whether its a separating axis.</param>
	/// <param name="x3">One of the remaining two points of quad1.</param>
	/// <param name="otherQuadPoints">The four points of the other quad.</param>
	/// <returns>Returns <c>true</c>, if the specified edge is a separating axis (and the quadrilaterals therefor don't 
	/// intersect). Returns <c>false</c>, if it's not a separating axis.</returns>
	bool DoAxisSeparationTest(Vector2 x1, Vector2 x2, Vector2 x3, Vector2[] otherQuadPoints) {
		Vector2 vec     = x2 - x1;
		Vector2 rotated = new Vector2(-vec.Y, vec.X);

		bool refSide = (rotated.X * (x3.X - x1.X)
					  + rotated.Y * (x3.Y - x1.Y)) >= 0;

		foreach (Vector2 pt in otherQuadPoints) {
			bool side = (rotated.X * (pt.X - x1.X) 
					   + rotated.Y * (pt.Y - x1.Y)) >= 0;
			if (side == refSide) {
				// At least one point of the other quad is one the same side as x3. Therefor the specified edge can't be a
				// separating axis anymore.
				return false;
			}
		}

		// All points of the other quad are on the other side of the edge. Therefor the edge is a separating axis and
		// the quads don't intersect.
		return true;
	}

	private Vector2 RotateVector2(Vector2 point) {
		return new Vector2(point.X * this._cos - point.Y * this._sin, point.Y * this._cos - point.X * this._sin);
	}
}
