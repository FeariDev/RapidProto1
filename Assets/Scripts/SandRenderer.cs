using UnityEngine;

public class SandRenderer : MonoBehaviour
{
    [Header("Sand Shader References")]
    public ComputeShader computeShader;
    public Renderer targetRenderer;
    ComputeBuffer sandTextureBuffer;
    ComputeBuffer nextTextureBuffer;
    ComputeShader computeShaderInstance;
    public RenderTexture sandRenderTexture;

    [Header("Sand Settings")]
    public int width;
    public int height;
    public float[] sandArray;
    ComputeBuffer sandGridBuffer;
    public float timeWait = 1f;
    public float timeDelta = 1f;

    [Header("Collider References")]
    public Camera colliderCamera;
    public RenderTexture colliderRenderTex;

    [Header("Collider Settings")]
    public LayerMask colliderLayer;
    public Vector2 colliderTexDebugSize;

    int kernelID;



    #region Unity lifecycle

    void Start()
    {
        InitializeSandRenderTexture();
        InitializeColliderRenderTexture();

        InitializeColliderCamera();

        InitializeComputeShader();

        targetRenderer.material.mainTexture = sandRenderTexture;
    }

    void Update()
    {
        UpdateColliderTexture();
        UpdateSandShader();
    }

    void OnDestroy()
    {
        if (sandRenderTexture != null) sandRenderTexture.Release();
        if (colliderRenderTex != null) colliderRenderTex.Release();

        if (Application.isPlaying)
        {
            Destroy(computeShaderInstance);
        }
        else
        {
            DestroyImmediate(computeShaderInstance);
        }

        sandTextureBuffer?.Release();
        nextTextureBuffer?.Release();
        sandGridBuffer?.Release();
    }

    #endregion



    #region Initialize methods

    void InitializeSandRenderTexture()
    {
        sandRenderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        sandRenderTexture.enableRandomWrite = true;
        sandRenderTexture.filterMode = FilterMode.Point;
        sandRenderTexture.Create();
    }

    void InitializeComputeShader()
    {
        computeShaderInstance = Instantiate(computeShader);

        kernelID = computeShaderInstance.FindKernel("CSMain");

        sandGridBuffer = new ComputeBuffer(width * height, sizeof(float));

        computeShaderInstance.SetBuffer(kernelID, "SandGrid", sandGridBuffer);

        sandGridBuffer.SetData(sandArray);

        computeShaderInstance.SetInt("width", width);
        computeShaderInstance.SetInt("height", height);
    }

    void InitializeColliderRenderTexture()
    {
        colliderRenderTex = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        colliderRenderTex.enableRandomWrite = true;
        colliderRenderTex.filterMode = FilterMode.Point;
        //colliderRenderTex.wrapMode = TextureWrapMode.Clamp;
        colliderRenderTex.Create();
    }
    void InitializeColliderCamera()
    {
        colliderCamera.orthographicSize = transform.localScale.x / 2;
        colliderCamera.targetTexture = colliderRenderTex;
    }

    #endregion



    #region Update methods

    void UpdateSandShader()
    {
        computeShaderInstance.SetTexture(kernelID, "SandTexture", sandRenderTexture);
        computeShaderInstance.SetTexture(kernelID, "ColliderTexture", colliderRenderTex);

        timeDelta += Time.deltaTime;
        if (timeDelta < timeWait) return;
        //computeShaderInstance.Dispatch(kernelID, width / 2, height / 2, 1);
        computeShaderInstance.Dispatch(kernelID, width, height, 1);
        timeDelta = 0;
    }

    void UpdateColliderTexture()
    {
        Graphics.SetRenderTarget(colliderRenderTex);
        GL.Clear(false, true, Color.black);

        colliderCamera.Render();
    }

    #endregion



    [Button]
    void ReadSandBuffer()
    {
        if (!Application.isPlaying) return;
        sandGridBuffer.GetData(sandArray);
    }



    void OnGUI()
    {
        if (colliderRenderTex != null) GUI.DrawTexture(new Rect(10, 10, width * colliderTexDebugSize.x, height * colliderTexDebugSize.y), colliderRenderTex);
    }
}
