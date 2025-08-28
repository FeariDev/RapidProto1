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
    public int threadGroupX;
    public int threadGroupY;
    public int[] sandArray;
    ComputeBuffer sandGridBuffer;
    public Vector2 sandSpawnPos;
    public int[] sandSpawnAmount;
    public int[] sandRemaining;
    public int[] sandCollected;
    ComputeBuffer sandSpawnAmtBuffer;
    ComputeBuffer sandRemainingBuffer;
    ComputeBuffer sandCollectedBuffer;
    public float timeWait = 1f;
    public float timeDelta = 1f;

    [Header("Hole Settings")]
    public Vector4 holeX;
    public Vector4 holeY;

    [Header("Collider References")]
    public Camera colliderCamera;
    public RenderTexture colliderRenderTex;

    [Header("Collider Settings")]
    public LayerMask colliderLayer;
    public Vector2 colliderTexDebugSize;

    [Header("Debug")]
    [SerializeField] bool showColliderMaskGUI = false;

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
        sandSpawnAmtBuffer?.Release();
        sandRemainingBuffer?.Release();
        sandCollectedBuffer?.Release();
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

        sandGridBuffer = new ComputeBuffer(width * height, sizeof(int));
        sandSpawnAmtBuffer = new ComputeBuffer(1, sizeof(int));
        sandRemainingBuffer = new ComputeBuffer(1, sizeof(int));
        sandCollectedBuffer = new ComputeBuffer(1, sizeof(int));

        computeShaderInstance.SetBuffer(kernelID, "SandGrid", sandGridBuffer);
        computeShaderInstance.SetBuffer(kernelID, "SandSpawnAmount", sandSpawnAmtBuffer);
        computeShaderInstance.SetBuffer(kernelID, "SandRemaining", sandRemainingBuffer);
        computeShaderInstance.SetBuffer(kernelID, "SandCollected", sandCollectedBuffer);

        //sandRemaining = sandSpawnAmount;
        sandGridBuffer.SetData(sandArray);
        sandSpawnAmtBuffer.SetData(sandSpawnAmount);
        sandRemainingBuffer.SetData(new int[1]);
        sandCollectedBuffer.SetData(new int[1]);

        computeShaderInstance.SetInt("width", width);
        computeShaderInstance.SetInt("height", height);

        computeShaderInstance.SetInt("sandSpawnX", (int)sandSpawnPos.x);
        computeShaderInstance.SetInt("sandSpawnY", (int)sandSpawnPos.y);

        computeShaderInstance.SetVector("holeX", holeX);
        computeShaderInstance.SetVector("holeY", holeY);
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

        computeShaderInstance.Dispatch(kernelID, width / threadGroupX, height / threadGroupY, 1);

        sandSpawnAmtBuffer.GetData(sandSpawnAmount);
        sandRemainingBuffer.GetData(sandRemaining);
        sandCollectedBuffer.GetData(sandCollected);
        //sandRemaining[0] = sandSpawnAmount[0] - sandCollected[0];

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
        if (showColliderMaskGUI && colliderRenderTex != null) GUI.DrawTexture(new Rect(10, 10, width * colliderTexDebugSize.x, height * colliderTexDebugSize.y), colliderRenderTex);
    }
}
