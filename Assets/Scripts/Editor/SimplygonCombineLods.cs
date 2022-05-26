using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System.Linq;
using UnityEditor;

namespace AdOne.Editor
{
    public class MeshRendererPair
    {
        public MeshFilter SourceM;
        public List<MeshFilter> TargetM;

        public MeshRendererPair(MeshFilter sourceM, List<MeshFilter> targetM)
        {
            SourceM = sourceM;
            TargetM = targetM;
        }
    }
    public class SkinnedMeshPair
    {
        public SkinnedMeshRenderer Source;
        public List<SkinnedMeshRenderer> Target;

        private string LogMessage;

        public SkinnedMeshPair(SkinnedMeshRenderer source, List<SkinnedMeshRenderer> target)
        {
            Source = source;
            Target = target;
        }

        public void DoMatch()
        {
            for (int i = 0; i < Target.Count; i++)
            {
                DoMatch(Source, Target[i]);
            }
        }

        public void DoMatch(SkinnedMeshRenderer source, SkinnedMeshRenderer target)
        {
            if (source == null || target == null)
                return;
            var sSke = source.bones;
            target.rootBone = source.rootBone;
            Transform[] newSke = new Transform[target.bones.Length];
            for (int i = 0; i < target.bones.Length; i++)
            {
                var find = sSke.FirstOrDefault(x => x.name == target.bones[i].name);
                newSke[i] = find;
            }
            target.bones = newSke;
        }
    }
    public class SimplygonCombineLods : OdinEditorWindow
    {
        #region Properties
        private List<SkinnedMeshPair> Pairs;
        private List<MeshRendererPair> PairsM;
        [Required]
        [InfoBox("Model Lod 0")]
        [PropertySpace(SpaceAfter = 10)]
        public GameObject Lod0;
        [InfoBox("Ném tất cả các GameObject source và lod sau khi giảm lưới vào đây, kết quả sẽ trả ra 1 GameObject trong scene\nCác GameObject có tên dạng xxx_LOD{X}")]
        [ListDrawerSettings(Expanded = true)]
        [PropertySpace(SpaceAfter = 10)]
        public List<GameObject> Inputs;
        #endregion

        #region Functions
        [MenuItem("AdOne/Combine Simplygon Lods")]
        private static void ShowWindow()
        {
            var window = GetWindow<SimplygonCombineLods>();

            // Nifty little trick to quickly position the window in the middle of the editor.
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(400, 700);
        }

        [Button("Combine!", ButtonHeight = 50)]
        private void DoMatchPairs()
        {
            if (Lod0 == null)
                return;
            ///prepairs
            ///
            Pairs = new List<SkinnedMeshPair>();
            var working = Inputs.ConvertAll(x => Instantiate(x).transform);
            Transform tSource = Instantiate(Lod0).transform;
            SkinnedMeshRenderer[] sSkinned = tSource.GetComponentsInChildren<SkinnedMeshRenderer>();
            List<SkinnedMeshRenderer> lodSkinned = new List<SkinnedMeshRenderer>();
            for (int i = 0; i < working.Count; i++)
            {
                lodSkinned = lodSkinned.Concat(working[i].GetComponentsInChildren<SkinnedMeshRenderer>().ToList()).ToList();
            }
            for (int i = 0; i < sSkinned.Length; i++)
            {
                List<SkinnedMeshRenderer> lods = lodSkinned.Where(x => x.name == sSkinned[i].name).ToList();
                sSkinned[i].name = $"{sSkinned[i].name}_LOD0";
                Pairs.Add(new SkinnedMeshPair(sSkinned[i], lods));
            }

            PairsM = new List<MeshRendererPair>();
            MeshFilter[] meshes = tSource.GetComponentsInChildren<MeshFilter>();
            List<MeshFilter> lodMeshes = new List<MeshFilter>();
            for (int i = 0; i < working.Count; i++)
            {
                lodMeshes = lodMeshes.Concat(working[i].GetComponentsInChildren<MeshFilter>().ToList()).ToList();
            }
            for (int i = 0; i < meshes.Length; i++)
            {
                List<MeshFilter> lods = lodMeshes.Where(x => x.name == meshes[i].name).ToList();
                meshes[i].name = $"{meshes[i].name}_LOD0";
                PairsM.Add(new MeshRendererPair(meshes[i], lods));
            }
            ///----------------------------

            DoMatchPairs(Pairs);

            ///export
            tSource.name = tSource.name.Replace("(Clone)", "") + "LODGroup";

            for (int i = 0; i < Pairs.Count; i++)
            {
                Pairs[i].Target.Sort((x, y) => { return y.sharedMesh.vertexCount - x.sharedMesh.vertexCount; });
                for (int j = 0; j < Pairs[i].Target.Count; j++)
                {
                    Pairs[i].Target[j].name = $"{Pairs[i].Target[j].name}_LOD{j+1}";
                    Pairs[i].Target[j].transform.parent = Pairs[i].Source.transform.parent;
                }
            }

            for (int i = 0; i < PairsM.Count; i++)
            {
                PairsM[i].TargetM.Sort((x, y) => { return y.sharedMesh.vertexCount - x.sharedMesh.vertexCount; });
                for (int j = 0; j < PairsM[i].TargetM.Count; j++)
                {
                    PairsM[i].TargetM[j].name = $"{PairsM[i].TargetM[j].name}_LOD{j + 1}";
                    PairsM[i].TargetM[j].transform.parent = PairsM[i].SourceM.transform.parent;
                }
            }
            working.ForEach(x => DestroyImmediate(x.gameObject));
        }

        private void DoMatchPairs(List<SkinnedMeshPair> pairs)
        {
            for (int i = 0; i < pairs.Count; i++)
            {
                pairs[i].DoMatch();
            }
        }
        #endregion
    }
}
