#if UNITY_EDITOR
using UnityEngine;


namespace net.rs64.TexTransTool.Decal.Curve
{
    [AddComponentMenu("TexTransTool/Experimental/CurveSegment")]
    public class CurveSegment : MonoBehaviour, ITexTransToolTag
    {
        [HideInInspector,SerializeField] int _saveDataVersion = Utils.ThiSaveDataVersion;
        public int SaveDataVersion => _saveDataVersion;
        public Vector3 position => transform.position;
        public float Roll = 0f;

    }
}
#endif