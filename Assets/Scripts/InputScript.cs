using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    [SerializeField] private Image selectionRect;
    [SerializeField] private RectTransform guiCanvasRect;
    [SerializeField] private List<Unit> selectedUnits = new List<Unit>();


    private Vector2 startMousePos;
    private Vector2 currentMousePos;

    private Vector2 startWorldMousePos;
    private Vector2 currentWorldMousePos;

    private UnitGroup unitGroup;

    private void Start()
    {
        startMousePos = Vector2.zero;
        currentMousePos = Vector2.zero;

        unitGroup = new UnitGroup();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            unitGroup.SetGroup(selectedUnits);
            unitGroup.MoveGroup(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        if (Input.GetMouseButtonDown(0))
        {
            selectionRect.gameObject.SetActive(true);
            startMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            currentMousePos = Input.mousePosition;
            UpdateSelection();
        }
        if (Input.GetMouseButtonUp(0))
        {
            selectionRect.gameObject.SetActive(false);
        }
    }

    private void OnGUI()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 screenStartMousePos = startMousePos;
            Vector2 screenCurrentMousePos = currentMousePos;

            screenStartMousePos.y = Mathf.Abs(Screen.height - screenStartMousePos.y);
            screenCurrentMousePos.y = Mathf.Abs(Screen.height - screenCurrentMousePos.y);

            float inset = Mathf.Min(screenStartMousePos.x, screenCurrentMousePos.x);
            float size = Mathf.Abs(screenCurrentMousePos.x - screenStartMousePos.x);
            selectionRect.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, size);

            inset = Mathf.Min(screenStartMousePos.y, screenCurrentMousePos.y);
            size = Mathf.Abs(screenCurrentMousePos.y - screenStartMousePos.y);
            selectionRect.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, size);
        }
    }

    private void UpdateSelection()
    {
        UnselectUnits();

        startWorldMousePos = Camera.main.ScreenToWorldPoint(startMousePos);
        currentWorldMousePos = Camera.main.ScreenToWorldPoint(currentMousePos);

        Collider2D[] colliders = Physics2D.OverlapAreaAll(startWorldMousePos, currentWorldMousePos);
        foreach (Collider2D collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (unit != null)
            {
                selectedUnits.Add(unit);
                unit.Select();
            }
        }
    }

    private void UnselectUnits()
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.Deselect();
        }
        selectedUnits.Clear();
    }




}
