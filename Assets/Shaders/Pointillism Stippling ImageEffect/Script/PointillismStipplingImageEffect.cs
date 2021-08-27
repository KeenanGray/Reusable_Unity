using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointillismStipplingImageEffect : MonoBehaviour
{
    public enum TYPE
    {
        COLORED,
        GRAYSCALE
    }

    [Range(0, 80)]
    public int blurSize = 9;

    [Range(1, 8)]
    public int brushSize = 4;

    public TYPE type = TYPE.COLORED;

    private Shader shader = null;

    private Material mtrl = null;

    private void Awake()
    {
        shader = Shader.Find("Hidden/PointillismStipplingImageEffect");
        if (!shader.isSupported)
        {
            enabled = false;
            return;
        }

        mtrl = new Material(shader);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (mtrl == null || mtrl.shader == null || !mtrl.shader.isSupported)
        {
            enabled = false;
            return;
        }

        if(blurSize == 0)
        {
            Graphics.Blit(src, dest);
            return;
        }

        RenderTexture rtTemp = RenderTexture.GetTemporary(src.width/brushSize, src.height/brushSize, 0, src.format);

        mtrl.SetInt("_BlurSize", blurSize);
        if(type == TYPE.COLORED)
        {
            mtrl.EnableKeyword("COLORED");
            mtrl.DisableKeyword("GRAYSCALE");
        }
        else
        {
            mtrl.EnableKeyword("GRAYSCALE");
            mtrl.DisableKeyword("COLORED");
        }
        Graphics.Blit(src, rtTemp, mtrl, 0);
        Graphics.Blit(rtTemp, dest);

        RenderTexture.ReleaseTemporary(rtTemp);
    }

    private void OnDestroy()
    {
        shader = null;

        if (mtrl != null)
        {
            DestroyImmediate(mtrl);
            mtrl = null;
        }
    }
}
