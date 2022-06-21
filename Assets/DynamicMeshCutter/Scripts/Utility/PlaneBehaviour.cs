
using Exploder.Utils;

namespace DynamicMeshCutter
{
    public class PlaneBehaviour : CutterBehaviour
    {
        public MeshTarget[] m_MeshTarget;
        public float DebugPlaneLength = 2;
        public void Cut()
        {
            var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
            {
                if (!root.activeInHierarchy)
                {
                    continue;
                }
                // else
                // {
                //     Helper.DebugLog("Name Root: " + root.name);
                // }
                    
                // var targets = root.GetComponentsInChildren<MeshTarget>();
                // foreach (var target in targets)
                // {
                //     Helper.DebugLog("Name: " + target.name);
                //     Cut(target, transform.position, transform.forward, null, OnCreated);
                //     ExploderSingleton.Instance.ExplodeObject(target.gameObject);
                // }

                for (int i = 0; i < m_MeshTarget.Length; i++)
                {
                    Cut(m_MeshTarget[i], transform.position, transform.forward, null, OnCreated);
                    ExploderSingleton.Instance.ExplodeObject(m_MeshTarget[i].gameObject);
                }
            }
        }

        void OnCreated(Info info, MeshCreationData cData)
        {
            MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
        }

    }
}


// namespace DynamicMeshCutter
// {
//     public class PlaneBehaviour : CutterBehaviour
//     {
//         public float DebugPlaneLength = 2;
//         public MeshTarget m_MeshTarget;
//         public void Cut()
//         {
//             // var roots = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
//             // foreach (var root in roots)
//             // {
//             //     if (!root.activeInHierarchy)
//             //         continue;
//             //     var targets = root.GetComponentsInChildren<MeshTarget>();
//             //     foreach (var target in targets)
//             //     {
//             //         Cut(target, transform.position, transform.forward, null, OnCreated);
//             //     }
//             // }
//             
//             Cut(m_MeshTarget, transform.position, transform.forward, null, OnCreated);
//             // ExploderSingleton.Instance.ExplodeObject(m_MeshTarget.gameObject);
//         }
//
//         void OnCreated(Info info, MeshCreationData cData)
//         {
//             MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
//         }
//
//     }
// }