using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer highlight;
    public bool isUnwalkable = false;
    public float unwalkableThreshold = 0.5f; // 30% of the tile covered by the line

    private void Awake() {
    
        highlight = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        highlight.color = new Color(1f, 1f, 1f, .5f);
    }

    private void OnMouseExit()
    {
        if(isUnwalkable) highlight.color = Color.red;
        else highlight.color = new Color(1f, 1f, 1f, 0f);
    }

    public void SetUnwalkable(bool unwalkable)
    {
        isUnwalkable = unwalkable;
        if (unwalkable)
        {
            // Change tile appearance to indicate it is unwalkable
            highlight.color = Color.red;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer != gameObject.layer)
        {
            // Get the edge collider of the line
            EdgeCollider2D lineCollider = collision.collider as EdgeCollider2D;

            // Calculate the overlap ratio (percentage of the line inside this tile)
            float overlapRatio = CalculateLineTileOverlap(lineCollider);

            if (overlapRatio >= unwalkableThreshold)
            {
                SetUnwalkable(true);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer != gameObject.layer)
        {
            SetUnwalkable(false);
        }
    }
    // This method calculates the amount of the line segment inside the tile
    private float CalculateLineTileOverlap(EdgeCollider2D lineCollider)
    {
        // Get the bounds of this tile (assuming it's a quad/rectangular)
        Bounds tileBounds = GetComponent<BoxCollider2D>().bounds;

        // Iterate through the points in the EdgeCollider2D
        float totalSegmentLength = 0f;
        float insideLength = 0f;
        if (lineCollider != null)
        {
            for (int i = 0; i < lineCollider.pointCount - 1; i++)
            {
                Vector2 pointA = lineCollider.points[i];
                Vector2 pointB = lineCollider.points[i + 1];

                float segmentLength = Vector2.Distance(pointA, pointB);
                totalSegmentLength += segmentLength;

                // Check if this segment intersects with the tile bounds
                if (tileBounds.IntersectRay(new Ray(pointA, pointB - pointA)))
                {
                    insideLength += segmentLength;
                }
            }
            return insideLength / totalSegmentLength;
        }
        else return 0;
        // Return the ratio of the length inside the tile to the total segment length
    }
}

