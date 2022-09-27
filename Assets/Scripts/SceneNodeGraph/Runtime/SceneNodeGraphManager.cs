//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace SceneNodeGraph
//{
//    public class SceneNodeGraphManager : MonoBehaviour
//    {
//        public RuntimeNodeGraph runtimeNodeGraph = new RuntimeNodeGraph();

//        private void Start()
//        {
//            if (runtimeNodeGraph == null) return;
//            SceneNodeGraphSerializer.TestLoad(runtimeNodeGraph.nodeGraphData);
//            runtimeNodeGraph.StartGraph();
//        }

//        private void Update()
//        {
//            if (runtimeNodeGraph == null) return;
//            if (!runtimeNodeGraph.IsFinished())
//                runtimeNodeGraph.UpdateNodes(Time.deltaTime);
//        }
//    }
//}

