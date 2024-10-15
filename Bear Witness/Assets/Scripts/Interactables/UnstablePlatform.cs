using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnstablePlatform : MonoBehaviour
{
    [SerializeField] int length;
    [SerializeField] Tilemap tilemap;
    [SerializeField] BoxCollider2D collider;

    [SerializeField] TileBase leftTile;
    [SerializeField] TileBase centerTile;
    [SerializeField] TileBase rightTile;

    private PlayerController player;

    private bool breaking = false;
    private bool regenerating = false;
    private float regenTime = 0f;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
        tilemap.animationFrameRate = 3f / Mathf.Sqrt(length);
        Debug.Log(tilemap.animationFrameRate);

        tilemap.SetTile(Vector3Int.right, leftTile);
        for (int i = 2; i <= length; i++)
        {
            if (i == length)
            {
                tilemap.SetTile(new Vector3Int(i, 0), rightTile);
            } else
            {
                tilemap.SetTile(new Vector3Int(i, 0), centerTile);
            }
        }

        collider.size = new Vector3(0.25f * length, 0.25f);
        collider.offset = new Vector3(0.25f * length / 2 + 0.25f, 0.125f);

        Debug.Log(collider.bounds);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(new Vector3(0.25f * length / 2 + 0.25f, 0.125f) + transform.position, new Vector3(0.25f * length, 0.25f));
    }

    private void Update()
    {
        if (breaking && tilemap.GetAnimationFrame(Vector3Int.right) == 2)
        {
            breaking = false;
            collider.enabled = false;
            for (int i = 1; i <= length; i++)
                tilemap.SetTileAnimationFlags(new Vector3Int(i, 0), TileAnimationFlags.PauseAnimation);
            regenerating = true;
            regenTime = Time.time + 4f / tilemap.animationFrameRate;
        }

        if (regenerating && regenTime < Time.time)
        {
            regenerating = false;
            for (int i = 1; i <= length; i++)
                tilemap.SetTileAnimationFlags(new Vector3Int(i, 0), TileAnimationFlags.LoopOnce);
            collider.enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!breaking && collision.collider.CompareTag("Player") && player.m_Grounded)
        {
            for (int i = 1; i <= length; i++)
            {
                tilemap.SetTileAnimationFlags(new Vector3Int(i, 0), TileAnimationFlags.None);
                tilemap.SetAnimationFrame(new Vector3Int(i, 0), 0);
            }


            breaking = true;
        }
    }
}
