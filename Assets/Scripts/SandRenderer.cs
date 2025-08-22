using UnityEngine;

public class SandRenderer : MonoBehaviour
{
    public ComputeShader computeShader;
    ComputeShader computeShaderInstance;

    public ComputeBuffer sandTextureBuffer;
    public ComputeBuffer nextTextureBuffer;
    public Renderer targetRenderer;
    RenderTexture sandRenderTexture;

    int kernelID;

    [Header("Settings")]
    public int width;
    public int height;
    public float[] sandArray;
    ComputeBuffer sandGridBuffer;



    void Start()
    {
        sandRenderTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        sandRenderTexture.enableRandomWrite = true;
        sandRenderTexture.filterMode = FilterMode.Point;
        sandRenderTexture.Create();

        InitializeComputeShader();

        targetRenderer.material.mainTexture = sandRenderTexture;
    }

    public float timeWait = 1f;
    public float timeDelta = 1f;
    void Update()
    {
        computeShaderInstance.SetTexture(kernelID, "SandTexture", sandRenderTexture);

        timeDelta += Time.deltaTime;
        if (timeDelta < timeWait) return;
        //computeShaderInstance.Dispatch(kernelID, width / 2, height / 2, 1);
        computeShaderInstance.Dispatch(kernelID, width, height, 1);
        timeDelta = 0;
    }

    void OnDestroy()
    {
        if (sandRenderTexture != null) sandRenderTexture.Release();

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

    [Button]
    void ReadSandBuffer()
    {
        if (!Application.isPlaying) return;
        sandGridBuffer.GetData(sandArray);
    }
}
