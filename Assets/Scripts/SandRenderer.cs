using UnityEngine;

public class SandRenderer : MonoBehaviour
{
    [Header("Sand Shader References")]
    [SerializeField] ComputeShader computeShader;
    [SerializeField] Renderer targetRenderer;
    ComputeBuffer sandTextureBuffer;
    ComputeBuffer nextTextureBuffer;
    ComputeShader computeShaderInstance;
    RenderTexture sandRenderTexture;

    /// <summary>
    /// The amount of sand that is available to spawn
    /// </summary>
    [Header("Sand Info")]
    [Tooltip("The amount of sand that is available to spawn")]
    [ReadOnly] public int sandSpawnAmount;
    /// <summary>
    /// The amount of sand there is currently visible on the map/sand renderer texture
    /// </summary>
    [Tooltip("The amount of sand there is currently visible on the map/sand renderer texture")]
    [ReadOnly] public int sandRemaining;
    /// <summary>
    /// The amount of sand that has gone in the hole/has been collected
    /// </summary>
    [Tooltip("The amount of sand that has gone in the hole/has been collected")]
    [ReadOnly] public int sandCollected;
    int[] sandSpawnAmountArray = new int[1];
    int[] sandRemainingArray = new int[1];
    int[] sandCollectedArray = new int[1];

    [Header("Sand Settings")]
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] Vector2 sandSpawnPos;
    /// <summary>
    /// The maximum amount of sand that can spawn
    /// </summary>
    public int sandMaxSpawnAmount;
    [SerializeField] int threadGroupX;
    [SerializeField] int threadGroupY;
    [SerializeField] int[] sandArray;
    ComputeBuffer sandGridBuffer;
    ComputeBuffer sandSpawnAmtBuffer;
    ComputeBuffer sandRemainingBuffer;
    ComputeBuffer sandCollectedBuffer;
    
    /// <summary>
    /// This represents how long it will take at the start of the game for the sand to start falling
    /// </summary>
    [Header("Sand timers")]
    [Tooltip("This represents how long it will take at the start of the game for the sand to start falling")]
    [SerializeField] float gracePeriod = 30f;
    [ReadOnly, SerializeField] float gracePeriodDelta;
    /// <summary>
    /// This represents how often the sand shader updates
    /// </summary>
    [Tooltip("This represents how often the sand shader updates")]
    [SerializeField] float sandUpdate = 0.1f;
    [ReadOnly, SerializeField] float sandUpdateDelta = 1f;
    
    

    [Header("Hole Settings")]
    [SerializeField] Vector4 holeX;
    [SerializeField] Vector4 holeY;

    [Header("Collider References")]
    [SerializeField] Camera colliderCamera;
    RenderTexture colliderRenderTex;

    [Header("Collider Settings")]
    [SerializeField] LayerMask colliderLayer;
    [SerializeField] Vector2 colliderTexDebugSize;

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

        InitializeBufferArrays();
        //sandRemaining = sandSpawnAmount;
        sandGridBuffer.SetData(sandArray);
        sandSpawnAmtBuffer.SetData(sandSpawnAmountArray);
        sandRemainingBuffer.SetData(new int[1]);
        sandCollectedBuffer.SetData(new int[1]);

        computeShaderInstance.SetInt("width", width);
        computeShaderInstance.SetInt("height", height);

        computeShaderInstance.SetInt("sandSpawnX", (int)sandSpawnPos.x);
        computeShaderInstance.SetInt("sandSpawnY", (int)sandSpawnPos.y);

        computeShaderInstance.SetVector("holeX", holeX);
        computeShaderInstance.SetVector("holeY", holeY);



        computeShaderInstance.SetTexture(kernelID, "SandTexture", sandRenderTexture);
        computeShaderInstance.SetTexture(kernelID, "ColliderTexture", colliderRenderTex);
    }
    void InitializeBufferArrays()
    {
        sandSpawnAmountArray[0] = sandMaxSpawnAmount;
        //sandRemainingArray[0] = ;
        //sandCollectedArray[0] = ;
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
        if (gracePeriodDelta < gracePeriod)
        {
            gracePeriodDelta += Time.deltaTime;
            return;
        }

        computeShaderInstance.SetTexture(kernelID, "SandTexture", sandRenderTexture);
        computeShaderInstance.SetTexture(kernelID, "ColliderTexture", colliderRenderTex);

        sandUpdateDelta += Time.deltaTime;
        if (sandUpdateDelta < sandUpdate) return;

        computeShaderInstance.Dispatch(kernelID, width / threadGroupX, height / threadGroupY, 1);

        sandSpawnAmtBuffer.GetData(sandSpawnAmountArray);
        sandRemainingBuffer.GetData(sandRemainingArray);
        sandCollectedBuffer.GetData(sandCollectedArray);
        UpdateInspectorBufferValues();

        sandUpdateDelta = 0;
    }

    void UpdateInspectorBufferValues()
    {
        sandSpawnAmount = sandSpawnAmountArray[0];
        sandRemaining = sandRemainingArray[0];
        sandCollected = sandCollectedArray[0];
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
        if (!showColliderMaskGUI) return;

        GUI.Box(new Rect(10, 10, width * colliderTexDebugSize.x, height * colliderTexDebugSize.y), "DEBUG");
        if (colliderRenderTex != null) GUI.DrawTexture(new Rect(10, 10, width * colliderTexDebugSize.x, height * colliderTexDebugSize.y), colliderRenderTex);
    }
}
