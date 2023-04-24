
using UnityEngine;


public interface IGroupInitializer
{
    //Maybe it can be deleted
    CircleCollider2D ScanArea { get; }
    bool IsInGroup { get; set; }


    AIGroup InitializeAIGroup();
}
