using LibGame.Physics;

namespace LibGame.Tests.Physics;

public static class TransformTests
{
    [Fact(DisplayName = "Smoke test for `Transform`")]
    public static void Smoke()
    {
        Transform t = default;

        // Sane defaults

        Equal(Quaternion.Identity, t.GetRotation());
        Equal(Vector3.Zero, t.GetPosition());
        Equal(-Vector3.UnitZ, t.GetForward());
        Equal(-Vector3.UnitX, t.GetLeft());
        Equal(Vector3.UnitY, t.GetUp());
        Equal(1.0f, t.GetScale());

        // Local and Global rotations

        t = default;

        t = t.AddLocalRotation(Quaternion.CreateFromYawPitchRoll(0, MathF.PI / 2, 0));

        EqualF(new Vector3(0, 1, 0), t.GetForward());

        t = t.AddLocalRotation(Quaternion.CreateFromYawPitchRoll(MathF.PI / 2, 0, 0));

        EqualF(new Vector3(-1, 0, 0), t.GetForward());

        t = t.AddRotation(Quaternion.CreateFromYawPitchRoll(0, 0, MathF.PI / 2));

        EqualF(new Vector3(0, -1, 0), t.GetForward());

        t = t.SetRotation(Quaternion.Identity);
        EqualF(new Vector3(0, 0, -1), t.GetForward());

        // Translations

        t = default;

        t = t.SetTranslation(Vector3.UnitY);
        EqualF(Vector3.UnitY, t.GetPosition());

        t = t.AddTranslation(Vector3.UnitY);
        EqualF(Vector3.UnitY * 2, t.GetPosition());

        // Combinding rotations and local translations

        t = default;

        t = t.SetRotation(Quaternion.CreateFromYawPitchRoll(MathF.PI / 2, 0, 0))
             .AddLocalTranslation(-Vector3.UnitZ);
        EqualF(-Vector3.UnitX, t.GetPosition());

        // Scaling

        t = default;

        t = t.AddScale(0.5f).AddScale(0.5f);
        Equal(0.25f, t.GetScale());

        t = t.SetScale(0.65f);
        Equal(0.65f, t.GetScale());

        // Utility methods

        t = default;
        t = t.FaceTarget(Vector3.UnitX);
        EqualF(Vector3.UnitX, t.GetForward());

        t = default;
        t = t.FaceTargetConstrained(Vector3.UnitX, Vector3.UnitY);
        EqualF(Vector3.UnitX, t.GetForward());
        EqualF(Vector3.UnitY, t.GetUp());

    }
}
