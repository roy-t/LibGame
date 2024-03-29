﻿using System.Numerics;

namespace LibGame.Physics;

public readonly struct Transform
{
    public static readonly Transform Identity = new Transform(Vector3.Zero, Quaternion.Identity, Vector3.Zero, 1.0f);

    private readonly Vector3 Position;
    private readonly Quaternion Rotation;
    private readonly Vector3 Origin;
    private readonly float Scale;
    private readonly bool IsSet;
    
    public Transform(Vector3 position, Quaternion rotation, Vector3 origin, float scale)
    {
        this.Position = position;
        this.Rotation = rotation;
        this.Origin = origin;
        this.Scale = scale;

        this.IsSet = true;
    }

    public readonly Matrix4x4 GetMatrix()
    {
        var moveToCenter = Matrix4x4.CreateTranslation(-this.Origin);
        var size = Matrix4x4.CreateScale(this.GetScale());
        var translation = Matrix4x4.CreateTranslation(this.Position);

        var rotationMatrix4x4 = Matrix4x4.CreateFromQuaternion(this.GetRotation());
        return size * moveToCenter * rotationMatrix4x4 * translation;
    }

    public readonly Quaternion GetRotation()
    {
        if (this.IsSet)
        {
            return this.Rotation;
        }

        return Quaternion.Identity;
    }

    public readonly Vector3 GetPosition()
    {
        return this.Position;
    }

    public readonly Vector3 GetForward()
    {
        return Vector3.Transform(-Vector3.UnitZ, this.GetRotation());
    }

    public readonly Vector3 GetUp()
    {
        return Vector3.Transform(Vector3.UnitY, this.GetRotation());
    }

    public readonly Vector3 GetLeft()
    {
        return Vector3.Transform(-Vector3.UnitX, this.GetRotation());
    }

    public readonly float GetScale()
    {
        if (this.IsSet)
        {
            return this.Scale;
        }

        return 1.0f;
    }

    public readonly Vector3 GetOrigin()
    {
        return this.Origin;
    }

    public readonly Transform SetOrigin(Vector3 origin)
    {
        return new Transform(this.Position, this.GetRotation(), origin, this.GetScale());
    }

    public readonly Transform SetRotation(Quaternion rotation)
    {
        return new Transform(this.Position, rotation, this.Origin, this.GetScale());
    }

    public readonly Transform AddRotation(Quaternion offset)
    {
        var q = Quaternion.Multiply(offset, this.GetRotation());
        return new Transform(this.Position, Quaternion.Normalize(q), this.Origin, this.GetScale());
    }

    public readonly Transform AddLocalRotation(Quaternion offset)
    {
        var q = Quaternion.Multiply(this.GetRotation(), offset);
        return new Transform(this.Position, Quaternion.Normalize(q), this.Origin, this.GetScale());
    }

    public readonly Transform SetTranslation(Vector3 translation)
    {
        return new Transform(translation, this.GetRotation(), this.Origin, this.GetScale());
    }

    public readonly Transform AddTranslation(Vector3 offset)
    {
        return new Transform(this.Position + offset, this.GetRotation(), this.Origin, this.GetScale());
    }    

    public readonly Transform AddLocalTranslation(Vector3 offset)
    {
        var vector = Vector3.Transform(offset, this.GetRotation());
        return this.AddTranslation(vector);
    }

    public readonly Transform SetScale(float scale)
    {
        return new Transform(this.Position, this.GetRotation(), this.Origin, scale);
    }

    public readonly Transform AddScale(float change)
    {
        return new Transform(this.Position, this.GetRotation(), this.Origin, this.GetScale() * change);
    }

    [Obsolete("Usually this function does not what you expect it to do, use FaceTargetContstrained instead.")]
    public readonly Transform FaceTarget(Vector3 target)
    {
        var currentForward = this.GetForward();
        var desiredForward = Vector3.Normalize(target - this.Position);

        var dot = Vector3.Dot(currentForward, desiredForward);

        Quaternion rotation;

        if (Math.Abs(dot - 1.0f) < 0.000001f)
        {
            // vector a and b point exactly in the same direction
            // so we do not need to do anything
            return this;
        }
        else if (Math.Abs(dot + 1.0f) < 0.000001f)
        {
            // vector a and b point exactly in the opposite direction, 
            // so it is a 180 degrees turn around the up-axis
            rotation = new Quaternion(Vector3.UnitY, MathF.PI);
        }
        else
        {
            var rotAngle = (float)Math.Acos(dot);
            var rotAxis = Vector3.Cross(currentForward, desiredForward);
            rotAxis = Vector3.Normalize(rotAxis);
            rotation = Quaternion.CreateFromAxisAngle(rotAxis, rotAngle);
        }

        var q = rotation * this.GetRotation();
        return new Transform(this.Position, Quaternion.Normalize(q), this.Origin, this.GetScale());
    }

    public readonly Transform FaceTargetConstrained(Vector3 target, Vector3 up)
    {
        var dot = Vector3.Dot(Vector3.Normalize(target - this.Position), up);
        if (Math.Abs(dot) < 0.995f)
        {
            var matrix = Matrix4x4.CreateLookAt(this.Position, target, up);
            if (Matrix4x4.Invert(matrix, out var inverted))
            {
                var q = Quaternion.CreateFromRotationMatrix(inverted);
                return new Transform(this.Position, Quaternion.Normalize(q), this.Origin, this.GetScale());
            }
        }

        return this;
    }
}
