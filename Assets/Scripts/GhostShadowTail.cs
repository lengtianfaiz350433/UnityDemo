using UnityEngine;
using System.Collections.Generic;
//[ExecuteInEditMode]
public class GhostShadowTail : MonoBehaviour {
    public bool Emit=true;
    [ColorUsage(true,true)]public Color _GhostShadowColor = Color.black;//残影颜色
    public float _SurvivalTime = 1;//生存时间
    public float _IntervalTime = 0.2f;//间隔时间
    public Material mat;//残影材质
    private float _Time = 0;//计时器                
    [Range(0.1f, 1.0f)] public float _InitialAlpha = 1.0f; //残影初始透明度
    private List<GhostShadow> _GhostShadowList;
    private SkinnedMeshRenderer _SkinnedMeshRenderer;
    //private MeshRenderer _MeshRenderer;
    void Awake () {
        _GhostShadowList = new List<GhostShadow>();
        _SkinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
    }
	void Update () {
        if (_SkinnedMeshRenderer != null)//如果有获取到蒙皮组件才开始绘制
        {
            _Time += Time.deltaTime;
            if (Emit)
            {
                CreateGhostShadow();//生成残影
            }
            UpdateGhostShadow();//刷新残影
        }
	}
 
    void CreateGhostShadow()
    {
        if (_Time >= _IntervalTime)//每次经过间隔时间就new一个模型
        {
            _Time = 0;
            // If no material assigned, skip creating a ghost to avoid null references
            if (mat == null) return;
            Mesh mesh = new Mesh();
                _SkinnedMeshRenderer.BakeMesh(mesh);//每一帧都要获取这个蒙皮后的mesh

            // Clone the inspector material so we don't destroy the shared asset
            Material material = new Material(mat);
            _GhostShadowList.Add(new GhostShadow( mesh,material,transform.localToWorldMatrix, _InitialAlpha,Time.realtimeSinceStartup, _SurvivalTime));
        }
    }

    void UpdateGhostShadow()//刷新残影，根据生存时间销毁已过时的残影
    {
        for (int i = 0; i < _GhostShadowList.Count; i++)
        {
            float _PassingTime = Time.realtimeSinceStartup - _GhostShadowList[i]._StartTime;
            if (_PassingTime > _GhostShadowList[i]._Duration)
            {
                // Cache reference before removing to avoid accessing shifted index
                GhostShadow expired = _GhostShadowList[i];
                _GhostShadowList.RemoveAt(i);
                // Destroy the underlying Unity objects created for the ghost
                if (expired._Mesh != null) Destroy(expired._Mesh);
                if (expired._Material != null) Destroy(expired._Material);
                // Adjust index because we removed the current element
                i--;
                continue;
            }
            if (_GhostShadowList[i]._Material.HasProperty("_Color"))
            {
                _GhostShadowList[i]._Alpha *= (1 - _PassingTime / _GhostShadowList[i]._Duration);
                _GhostShadowColor.a = _GhostShadowList[i]._Alpha;
                _GhostShadowList[i]._Material.SetColor("_Color", _GhostShadowColor);
            }
            Graphics.DrawMesh(_GhostShadowList[i]._Mesh, _GhostShadowList[i]._Matrix, _GhostShadowList[i]._Material, gameObject.layer);
        }
    }
}
class GhostShadow : Object
{
    public Mesh _Mesh;//残影网格
    public Material _Material;//残影纹理
    public Matrix4x4 _Matrix;//残影位置
    public float _Alpha;//残影透明度
    public float _StartTime;//残影启动时间
    public float _Duration;//残影保留时间
    public GhostShadow(Mesh mesh, Material material, Matrix4x4 matrix4x4, float alpha, float startTime, float duration)
    {
        _Mesh = mesh;
        _Material = material;
        _Matrix = matrix4x4;
        _Alpha = alpha;
        _StartTime = startTime;
        _Duration = duration;
    }
}
