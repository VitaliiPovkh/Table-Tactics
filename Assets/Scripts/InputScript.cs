using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    [SerializeField] private Image selectionRect;
    [SerializeField] private RectTransform guiCanvasRect;
    [SerializeField] private List<Selectable> selectedUnits = new List<Selectable>();
    [Range(0f, 30f)]
    [SerializeField] private float formationRadius = 15f;

    private Vector2 selectionStartMousePos;
    private Vector2 selectionEndMousePos;

    private UnitGroup unitGroup;

    private void Start()
    {
        selectionStartMousePos = Vector2.zero;
        selectionEndMousePos = Vector2.zero;

        unitGroup = new UnitGroup();
    }

    /*TODO:
     * - Implement movement marker gameobject, so that only AIDestinationSetter could be used 
     * - Fix one click selection (now it selects all of the units in click area)
     **/
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            unitGroup.SetGroup(selectedUnits);

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D[] objectHit = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero, 20f);
            List<RaycastHit2D> listOfHits = new List<RaycastHit2D>(objectHit);

            Enemy enemy = null;
            listOfHits.Find((hit) => hit.collider.TryGetComponent(out enemy));

            if (enemy != null)
            {
                unitGroup.Attack(enemy);
                return;
            }

            unitGroup.MoveGroup(mouseWorldPos, formationRadius);
        }
        if (Input.GetMouseButtonDown(0))
        {
            selectionRect.gameObject.SetActive(true);
            selectionStartMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            selectionEndMousePos = Input.mousePosition;
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
            DrawSelectionRectangle();
        }
    }

    private void UpdateSelection()
    {
        UnselectUnits();

        Vector2 selectionStartWorldMousePos = Camera.main.ScreenToWorldPoint(selectionStartMousePos);
        Vector2 selectionEndWorldMousePos = Camera.main.ScreenToWorldPoint(selectionEndMousePos);

        Collider2D[] colliders = Physics2D.OverlapAreaAll(selectionStartWorldMousePos, selectionEndWorldMousePos);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Selectable unit))
            {
                selectedUnits.Add(unit);
                unit.Select();
            }
        }
    }

    private void UnselectUnits()
    {
        foreach (Selectable unit in selectedUnits)
        {
            unit.Deselect();
        }
        selectedUnits.Clear();
    }

    private void DrawSelectionRectangle()
    {
        Vector2 screenStartMousePos = selectionStartMousePos;
        Vector2 screenEndMousePos = selectionEndMousePos;

        screenStartMousePos.y = Mathf.Abs(Screen.height - screenStartMousePos.y);
        screenEndMousePos.y = Mathf.Abs(Screen.height - screenEndMousePos.y);

        float inset = Mathf.Min(screenStartMousePos.x, screenEndMousePos.x);
        float size = Mathf.Abs(screenEndMousePos.x - screenStartMousePos.x);
        selectionRect.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, size);

        inset = Mathf.Min(screenStartMousePos.y, screenEndMousePos.y);
        size = Mathf.Abs(screenEndMousePos.y - screenStartMousePos.y);
        selectionRect.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, size);
    }


}
