﻿using Sandbox;
using Sandbox.Diagnostics;

public abstract class ColliderBaseComponent : GameObjectComponent
{
	PhysicsShape shape;
	PhysicsBody ownBody;

	public override void OnEnabled()
	{
		Assert.IsNull( ownBody );
		Assert.IsNull( shape );

		PhysicsBody physicsBody = null;

		// is there a physics body?
		var body = GameObject.GetComponentInParent<PhysicsComponent>();
		if ( body is not null )
		{
			physicsBody = body.GetBody();

			//
			if ( physicsBody is null )
			{
				Log.Warning( $"{this}: PhysicsBody from {body} was null" );
				return;
			}

		}
		
		if ( physicsBody is null )
		{
			physicsBody = new PhysicsBody( Scene.PhysicsWorld );
			physicsBody.BodyType = PhysicsBodyType.Keyframed;
			physicsBody.UseController = true;
			physicsBody.GameObject = GameObject;
			physicsBody.Transform = GameObject.WorldTransform;
			physicsBody.GravityEnabled = false;
			ownBody = physicsBody;
		}

		shape = CreatePhysicsShape( physicsBody );
	}

	protected abstract PhysicsShape CreatePhysicsShape( PhysicsBody targetBody );

	public override void OnDisabled()
	{
		//shape?.Body?.RemoveShape( shape );
		shape?.Remove();
		shape = null;

		ownBody?.Remove();
		ownBody = null;
	}

	protected override void OnPostPhysics()
	{
		if ( ownBody is not null )
		{
			ownBody.Transform = GameObject.WorldTransform;
		}
	}
}