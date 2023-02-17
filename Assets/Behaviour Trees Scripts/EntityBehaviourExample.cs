using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviourExample : MonoBehaviour
{
    TreeRoot mRoot;
    // Start is called before the first frame update
    void Start()
    {
        mRoot = new TreeRoot();
        TreeNodes Sequence1 = new TreeNodes("Sequence1");
        TreeNodes Action1 = new TreeNodes("Action1");
        TreeNodes Action2 = new TreeNodes("Action2");
        
        Sequence1.AddChild(Action1);
        Sequence1.AddChild(Action2);
        mRoot.AddChild(Sequence1);

        TreeNodes Sequence2 = new TreeNodes("Sequence2");
        TreeNodes Action3 = new TreeNodes("Action3");
        TreeNodes Action4 = new TreeNodes("Action4");

        Sequence2.AddChild(Action3);
        Sequence2.AddChild(Action4);
        mRoot.AddChild(Sequence2);

        mRoot.PrintTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
