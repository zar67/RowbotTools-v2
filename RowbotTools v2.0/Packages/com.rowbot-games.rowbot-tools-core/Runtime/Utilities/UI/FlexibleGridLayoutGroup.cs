namespace RowbotTools.Core.Utilities
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A layout group component for displaying elements in a grid, but in a flexible way similar to the
    /// horizontal and vertical layout groups, instead of the default grid layout group.
    /// </summary>
    public class FlexibleGridLayoutGroup : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        [SerializeField] private FitType m_fitType;
        [SerializeField] private Vector2 m_spacing;
        [SerializeField] private int m_rows;
        [SerializeField] private int m_columns;
        [SerializeField] private Vector2 m_cellSize;

        [SerializeField] private bool m_fitX;
        [SerializeField] private bool m_fitY;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (m_fitType == FitType.Uniform || m_fitType == FitType.Width || m_fitType == FitType.Height)
            {
                m_fitX = true;
                m_fitY = true;

                float sqrRt = Mathf.Sqrt(transform.childCount);
                m_rows = Mathf.CeilToInt(sqrRt);
                m_columns = Mathf.CeilToInt(sqrRt);
            }

            if (m_fitType == FitType.Width || m_fitType == FitType.FixedColumns)
            {
                m_rows = Mathf.CeilToInt(transform.childCount / (float)m_columns);
            }
            else if (m_fitType == FitType.Height || m_fitType == FitType.FixedRows)
            {
                m_columns = Mathf.CeilToInt(transform.childCount / (float)m_rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = (parentWidth / m_columns) - (m_spacing.x / m_columns * (m_columns - 1)) - (padding.left / (float)m_columns) - (padding.right / (float)m_columns);
            float cellHeight = (parentHeight / m_rows) - (m_spacing.y / m_rows * (m_rows - 1)) - (padding.top / (float)m_rows) - (padding.bottom / (float)m_rows);

            m_cellSize.x = m_fitX ? cellWidth : m_cellSize.x;
            m_cellSize.y = m_fitY ? cellHeight : m_cellSize.y;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var item = rectChildren[i];

                int rowCount = i / m_columns;
                int columnCount = i % m_columns;

                float xPosition = (m_cellSize.x * columnCount) + (m_spacing.x * columnCount) + padding.left;
                float yPosition = (m_cellSize.y * rowCount) + (m_spacing.y * rowCount) + padding.top;
                

                SetChildAlongAxis(item, 0, xPosition, m_cellSize.x);
                SetChildAlongAxis(item, 1, yPosition, m_cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {

        }

        public override void SetLayoutHorizontal()
        {

        }

        public override void SetLayoutVertical()
        {

        }
    }
}