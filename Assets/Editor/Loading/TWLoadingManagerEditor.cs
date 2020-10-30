/* TWLoading version 1.0
 * Update Date : 29/08/2020
 * Created by TomWill
 */
using UnityEditor;
using TomWill;

[CustomEditor(typeof(TWLoadingManager))]
public class TWLoadingManagerEditor : Editor
{
    TWLoadingManager manager;

    SerializedProperty loadingController, loadingImageLib, loadingFrames, currentFrame, fps;

    private void OnEnable()
    {
        loadingController = serializedObject.FindProperty("loadingController");
        loadingImageLib = serializedObject.FindProperty("loadingImageLib");
        loadingFrames = serializedObject.FindProperty("loadingFrames");
        currentFrame = serializedObject.FindProperty("currentFrame");
        fps = serializedObject.FindProperty("fps");
    }

    public override void OnInspectorGUI()
    {
        manager = (TWLoadingManager) target;

        serializedObject.Update();

        ShowProperties();

        serializedObject.ApplyModifiedProperties();
    }

    void ShowProperties()
    {
        EditorGUILayout.PropertyField(loadingController);
        EditorGUILayout.PropertyField(loadingImageLib);
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Loading Animation", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(loadingFrames);

        if (manager.FrameLength() > 0)
        {
            EditorGUILayout.Space(2);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Frame");
            currentFrame.intValue = EditorGUILayout.IntSlider(currentFrame.intValue, 0, manager.FrameLength() - 1);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(2);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("FPS");
            fps.intValue = EditorGUILayout.IntSlider(fps.intValue, 0, manager.FrameLength());
            EditorGUILayout.EndHorizontal();
        } else
        {
            currentFrame.intValue = 0;
            fps.intValue = 0;
        }
    }
}
