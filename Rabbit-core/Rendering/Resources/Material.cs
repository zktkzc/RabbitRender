using OpenTK.Graphics.OpenGL4;

namespace Rabbit_core.Rendering.Resources;

public class Material
{
    
}

public struct RasterizationSettings
{
    public BlendableOptions BlendableOptions;
    public CullingOptions CullingOptions;
    public DepthTestOptions DepthTestOptions;
    public ColorWritingOptions ColorWritingOptions;

    public RasterizationSettings()
    {
        BlendableOptions = new BlendableOptions();
        CullingOptions = new CullingOptions();
        DepthTestOptions = new DepthTestOptions();
        ColorWritingOptions = new ColorWritingOptions();
    }
}

public struct BlendableOptions
{
    public bool IsBlendable; // 是否开启混合
    public BlendingFactor SFactor;
    public BlendingFactor DFactor;

    public BlendableOptions()
    {
        IsBlendable = true;
        SFactor = BlendingFactor.Src1Alpha;
        DFactor = BlendingFactor.OneMinusSrcAlpha;
    }
}

public struct CullingOptions
{
    public bool IsBackFaceCulling;
    public bool IsFrontFaceCulling;

    public CullingOptions()
    {
        IsBackFaceCulling = true;
        IsFrontFaceCulling = false;
    }
}

public struct DepthTestOptions
{
    public bool IsEnableDepthTest;
    public bool IsEnableDepthWriting;
    public DepthFunction DepthFunction;

    public DepthTestOptions()
    {
        IsEnableDepthTest = true;
        IsEnableDepthWriting = true;
        DepthFunction = DepthFunction.Less;
    }
}

public struct ColorWritingOptions
{
    public bool R;
    public bool G;
    public bool B;
    public bool A;

    public ColorWritingOptions()
    {
        R = G = B = A = true;
    }
}