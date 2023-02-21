using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    [SerializeField] private Image selectionRect;
    [SerializeField] private List<Unit> selectedUnits = new List<Unit>();


    private Vector2 startMousePos;
    private Vector2 currentMousePos;

    private Vector2 startWorldMousePos;
    private Vector2 currentWorldMousePos;


    private void Start()
    {
        startMousePos = Vector2.zero;
        currentMousePos = Vector2.zero;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (Unit unit in selectedUnits)
            {
                unit.MovementDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            selectionRect.gameObject.SetActive(true);
            startMousePos = Input.mousePosition;
            startMousePos.y = Mathf.Abs(Screen.height - startMousePos.y);
        }
        if (Input.GetMouseButton(0))
        {
            currentMousePos = Input.mousePosition;
            currentMousePos.y = Mathf.Abs(Screen.height - currentMousePos.y);
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
            float inset = Mathf.Min(startMousePos.x, currentMousePos.x);
            float size = Mathf.Abs(currentMousePos.x - startMousePos.x);
            selectionRect.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, inset, size);

            inset = Mathf.Min(startMousePos.y, currentMousePos.y);
            size = Mathf.Abs(currentMousePos.y - startMousePos.y);
            selectionRect.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, inset, size);
        }
    }

    private void UpdateSelection()
    {
        UnselectUnits();

        //Баг с выделением
        startWorldMousePos = Camera.main.ScreenToWorldPoint(startMousePos);
        startWorldMousePos.y *= -1;
        currentWorldMousePos = Camera.main.ScreenToWorldPoint(currentMousePos);
        currentWorldMousePos.y *= -1;


        Debug.Log($"Start pos {startWorldMousePos}");
        Debug.Log($"Current pos {currentWorldMousePos}");

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
