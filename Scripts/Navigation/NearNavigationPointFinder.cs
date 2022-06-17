using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavigationPoint))]
public class NearNavigationPointFinder : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Autoconnector");

        NavigationPoint NP = (NavigationPoint)target;

        // Inside Editor button which highlights next navigationpoints ways
        if (GUILayout.Button("Mark/Hide connections"))
        {
            NP.MarkConnections();
        }

        // Inside Edtor button which can save a lot of unnessesery clicking and adding next navigation points in inspector to few clicks and ctrl+z
        if (GUILayout.Button("Find New Connection Point"))
        {
            NP.FindNewNavigationPoint();


            Selection.activeObject = NP.NextNavigationPoints[NP.AutoConnectorSelectedNaviPoint];

            PrefabUtility.RecordPrefabInstancePropertyModifications(NP);

        }

        DrawDefaultInspector();
    }
}