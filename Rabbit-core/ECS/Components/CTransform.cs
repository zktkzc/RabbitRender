using OpenTK.Mathematics;

namespace Rabbit_core.ECS.Components;

public class CTransform : IComponent
{
    public Guid Id { get; }
    
    public Vector3 LocalPosition
    {
        get => GetLocalPosition();
        set => SetLocalPosition(value);
    }

    public Quaternion LocalRotation
    {
        get => GetLocalRotation();
        set => SetLocalRotation(value);
    }

    public Vector3 LocalScale
    {
        get => GetLocalScale();
        set => SetLocalScale(value);
    }

    public Vector3 WorldPosition => GetWorldPosition();
    public Quaternion WorldRotation => GetWorldRotation();
    public Vector3 WorldScale => GetWorldScale();

    public Vector3 LocalForward => _localRotation * Vector3.UnitZ;
    public Vector3 LocalUp => _localRotation * Vector3.UnitY;
    public Vector3 LocalRight => _localRotation * Vector3.UnitX;
    
    public Vector3 WorldForward => WorldRotation * Vector3.UnitZ;
    public Vector3 WorldUp => WorldRotation * Vector3.UnitY;
    public Vector3 WorldRight => WorldRotation * Vector3.UnitX;

    public CTransform(Guid id)
    {
        Id = id;

        _localPosition = new Vector3();
        _localRotation = Quaternion.Identity;
        _localScale = Vector3.One;
        _parentMatrix = Matrix4.Identity;
        _localMatrix = Matrix4.Identity;
        _worldMatrix = Matrix4.Identity;
        _isDirty = false;
    }

    private Vector3 _localPosition;
    private Quaternion _localRotation;
    private Vector3 _localScale;
    private Matrix4 _parentMatrix;
    private Matrix4 _localMatrix;
    private Matrix4 _worldMatrix;
    private bool _isDirty; // 判断局部信息是否更改

    public void SetLocalPosition(Vector3 localPosition)
    {
        _localPosition = localPosition;
        _isDirty = true;
    }

    public Vector3 GetLocalPosition() => _localPosition;

    public void SetLocalRotation(Quaternion localRotation)
    {
        _localRotation = localRotation;
        _isDirty = true;
    }

    public Quaternion GetLocalRotation() => _localRotation;

    public void SetLocalScale(Vector3 localScale)
    {
        _localScale = localScale;
        _isDirty = true;
    }

    public Vector3 GetLocalScale() => _localScale;

    /// <summary>
    /// 更新矩阵
    /// </summary>
    private void UpdateMatrices()
    {
        _localMatrix = Matrix4.CreateScale(_localScale) * Matrix4.CreateFromQuaternion(_localRotation) *
                       Matrix4.CreateTranslation(_localPosition);
        _worldMatrix = _parentMatrix * _localMatrix;
        _isDirty = false;
    }

    /// <summary>
    /// 设置世界矩阵
    /// </summary>
    /// <param name="transform"></param>
    internal void SetWorldMatrix(Matrix4 transform)
    {
        if (_isDirty)
        {
            UpdateMatrices();
        }
        _parentMatrix = transform;
        _worldMatrix = _parentMatrix * _localMatrix;
    }

    /// <summary>
    /// 设置局部矩阵
    /// </summary>
    /// <param name="transform"></param>
    public void SetLocalMatrix(Matrix4 transform)
    {
        _localMatrix = transform;
        _worldMatrix = _parentMatrix * _localMatrix;
        ApplyTransform();
    }

    public Matrix4 GetWorldMatrix()
    {
        if (_isDirty) UpdateMatrices();
        return _worldMatrix;
    }

    public Matrix4 GetLocalMatrix()
    {
        if (_isDirty)UpdateMatrices();
        return _localMatrix;
    }

    public Vector3 GetWorldPosition()
    {
        if (_isDirty) UpdateMatrices();
        return _worldMatrix.ExtractTranslation();
    }
    
    public Quaternion GetWorldRotation()
    {
        if (_isDirty) UpdateMatrices();
        return _worldMatrix.ExtractRotation();
    }
    
    public Vector3 GetWorldScale()
    {
        if (_isDirty) UpdateMatrices();
        return _worldMatrix.ExtractScale();
    }

    private void ApplyTransform()
    {
        _localPosition = _localMatrix.ExtractTranslation();
        _localRotation = _localMatrix.ExtractRotation();
        _localScale = _localMatrix.ExtractScale();
    }
}