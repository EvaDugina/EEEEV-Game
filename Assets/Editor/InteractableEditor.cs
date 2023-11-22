using UnityEditor;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactable interactable = (Interactable)target;
        if (target.GetType() == typeof(EventOnlyInteractable))
        {
            interactable.promtMessage = EditorGUILayout.TextField("Promt Message", interactable.promtMessage);
            EditorGUILayout.HelpBox("EventOnlyInteractable can only use UnityEvents", MessageType.Info);
            if (interactable.GetComponent<IntarationEvent>() == null)
            {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<IntarationEvent>();
            }
        }
        else
        {


            base.OnInspectorGUI();

            if (interactable.useEvents)
            {
                if (interactable.gameObject.GetComponent<IntarationEvent>() == null)
                {
                    interactable.gameObject.AddComponent<IntarationEvent>();
                }

            }
            else
            {
                if (interactable.gameObject.GetComponent<IntarationEvent>() != null)
                {
                    DestroyImmediate(interactable.GetComponent<IntarationEvent>());
                }
            }
        }
    }
}
