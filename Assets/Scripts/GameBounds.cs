//
//  GameBounds.cs
//  
//  Dev: Scott Mitchell
//  Date: 09.05.24.
//

using UnityEngine;

sealed public class GameBounds : MonoBehaviour
{
    [SerializeField] private float m_Width;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(0f, 2.5f, 0f), new Vector3()
        {
            x = m_Width, y = 0.5f
        });
    }
}