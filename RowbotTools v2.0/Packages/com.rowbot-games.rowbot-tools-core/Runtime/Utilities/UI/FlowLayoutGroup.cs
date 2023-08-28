namespace RowbotTools.Core.Utilities
{
	using System.Collections.Generic;
	using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Layout Group controller that arranges children in bars, fitting as many on a line until total size exceeds parent bounds
    /// </summary>
	public class FlowLayoutGroup : LayoutGroup
	{
		public enum Axis
		{
			Horizontal = 0, Vertical = 1
		}

		[SerializeField] protected Axis m_startAxis = Axis.Horizontal;
		[SerializeField] private Vector2 m_spacing;
		
		[SerializeField] private bool m_flushAlignHorizontal;
		[SerializeField] private bool m_childForceExpandWidth;
		[SerializeField] private bool m_childForceExpandHeight;

		[SerializeField] private bool m_invertOrder = false;

		private float m_layoutHeight;
		private float m_layoutWidth;

		private bool IsCenterAlign => childAlignment == TextAnchor.LowerCenter || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.UpperCenter;

		private bool IsRightAlign => childAlignment == TextAnchor.LowerRight || childAlignment == TextAnchor.MiddleRight || childAlignment == TextAnchor.UpperRight;

		private bool IsMiddleAlign => childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.MiddleRight || childAlignment == TextAnchor.MiddleCenter;

		private bool IsLowerAlign => childAlignment == TextAnchor.LowerLeft || childAlignment == TextAnchor.LowerRight || childAlignment == TextAnchor.LowerCenter;

		/// <summary>
		/// Holds the rects that will make up the current bar being processed
		/// </summary>
		private readonly IList<RectTransform> m_itemList = new List<RectTransform>();

		public override void CalculateLayoutInputHorizontal()
		{
			if (m_startAxis == Axis.Horizontal)
			{
				base.CalculateLayoutInputHorizontal();
				var minWidth = GetGreatestMinimumChildWidth() + padding.left + padding.right;
				SetLayoutInputForAxis(minWidth, -1, -1, 0);
			}
			else
			{
				m_layoutWidth = SetLayout(0, true);
			}

		}

		public override void CalculateLayoutInputVertical()
		{
			if (m_startAxis == Axis.Horizontal)
			{
				m_layoutHeight = SetLayout(1, true);
			}
			else
			{
				base.CalculateLayoutInputHorizontal();
				var minHeight = GetGreatestMinimumChildHeight() + padding.bottom + padding.top;
				SetLayoutInputForAxis(minHeight, -1, -1, 1);
			}
		}

		public override void SetLayoutHorizontal()
		{
			SetLayout(0, false);
		}

		public override void SetLayoutVertical()
		{
			SetLayout(1, false);
		}

		protected override void OnDisable()
		{
			m_Tracker.Clear();
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}

		private float SetLayout(int axis, bool layoutInput)
		{
			var groupHeight = rectTransform.rect.height;
			var groupWidth = rectTransform.rect.width;

			float spacingBetweenBars = 0;
			float spacingBetweenElements = 0;
			float offset = 0;
			float counterOffset = 0;
			float groupSize = 0;
			float workingSize = 0;

			if (m_startAxis == Axis.Horizontal)
			{
				groupSize = groupHeight;
				workingSize = groupWidth - padding.left - padding.right;

				offset = IsLowerAlign ? padding.bottom : padding.top;
				counterOffset = IsLowerAlign ? padding.top : padding.bottom;

				spacingBetweenBars = m_spacing.y;
				spacingBetweenElements = m_spacing.x;
			}
			else if (m_startAxis == Axis.Vertical)
			{
				groupSize = groupWidth;
				workingSize = groupHeight - padding.top - padding.bottom;

				offset = IsRightAlign ? padding.right : padding.left;
				counterOffset = IsRightAlign ? padding.left : padding.right;

				spacingBetweenBars = m_spacing.x;
				spacingBetweenElements = m_spacing.y;
			}

			var currentBarSize = 0f;
			var currentBarSpace = 0f;

			for (var i = 0; i < rectChildren.Count; i++)
			{
				int index = i;
				var child = rectChildren[index];
				float childSize = 0;
				float childOtherSize = 0;

				if (m_startAxis == Axis.Horizontal)
				{
					if (m_invertOrder)
					{
						index = IsLowerAlign ? rectChildren.Count - 1 - i : i;
					}

					child = rectChildren[index];
					childSize = LayoutUtility.GetPreferredSize(child, 0);
					childSize = Mathf.Min(childSize, workingSize);
					childOtherSize = LayoutUtility.GetPreferredSize(child, 1);
				}
				else if (m_startAxis == Axis.Vertical)
				{
					if (m_invertOrder)
					{
						index = IsRightAlign ? rectChildren.Count - 1 - i : i;
					}

					child = rectChildren[index];
					childSize = LayoutUtility.GetPreferredSize(child, 1);
					childSize = Mathf.Min(childSize, workingSize);
					childOtherSize = LayoutUtility.GetPreferredSize(child, 0);
				}

				// If adding this element would exceed the bounds of the container, go to a new bar after processing the current bar
				if (currentBarSize + childSize > workingSize)
				{
					currentBarSize -= spacingBetweenElements;

					// Process current bar elements positioning
					if (!layoutInput)
					{
						if (m_startAxis == Axis.Horizontal)
						{
							float newOffset = CalculateRowVerticalOffset(groupSize, offset, currentBarSpace);
							LayoutRow(m_itemList, currentBarSize, currentBarSpace, workingSize, padding.left, newOffset, axis);
						}
						else if (m_startAxis == Axis.Vertical)
						{
							float newOffset = CalculateColHorizontalOffset(groupSize, offset, currentBarSpace);
							LayoutCol(m_itemList, currentBarSpace, currentBarSize, workingSize, newOffset, padding.top, axis);
						}
					}

					// Clear existing bar
					m_itemList.Clear();

					// Add the current bar space to total barSpace accumulator, and reset to 0 for the next row
					offset += currentBarSpace;
					offset += spacingBetweenBars;

					currentBarSpace = 0;
					currentBarSize = 0;
				}

				currentBarSize += childSize;
				m_itemList.Add(child);

				// We need the largest element height to determine the starting position of the next line
				if (childOtherSize > currentBarSpace)
				{
					currentBarSpace = childOtherSize;
				}

				// Don't do this for the last one
				if (i < rectChildren.Count - 1)
				{
					currentBarSize += spacingBetweenElements;
				}
			}

			// Layout the final bar
			if (!layoutInput)
			{
				if (m_startAxis == Axis.Horizontal)
				{
					float newOffset = CalculateRowVerticalOffset(groupHeight, offset, currentBarSpace);
					currentBarSize -= spacingBetweenElements;
					LayoutRow(m_itemList, currentBarSize, currentBarSpace, workingSize, padding.left, newOffset, axis);
				}
				else if (m_startAxis == Axis.Vertical)
				{
					float newOffset = CalculateColHorizontalOffset(groupWidth, offset, currentBarSpace);
					currentBarSize -= spacingBetweenElements;
					LayoutCol(m_itemList, currentBarSpace, currentBarSize, workingSize, newOffset, padding.top, axis);
				}
			}

			m_itemList.Clear();

			// Add the last bar space to the barSpace accumulator
			offset += currentBarSpace;
			offset += counterOffset;

			if (layoutInput)
			{
				SetLayoutInputForAxis(offset, offset, -1, axis);
			}

			return offset;
		}

		private float CalculateRowVerticalOffset(float groupHeight, float yOffset, float currentRowHeight)
		{
			if (IsLowerAlign)
			{
				return groupHeight - yOffset - currentRowHeight;
			}
			else if (IsMiddleAlign)
			{
				return (groupHeight * 0.5f) - (m_layoutHeight * 0.5f) + yOffset;
			}
			else
			{
				return yOffset;
			}
		}

		private float CalculateColHorizontalOffset(float groupWidth, float xOffset, float currentColWidth)
		{
			if (IsRightAlign)
			{
				return groupWidth - xOffset - currentColWidth;
			}
			else if (IsCenterAlign)
			{
				return (groupWidth * 0.5f) - (m_layoutWidth * 0.5f) + xOffset;
			}
			else
			{
				return xOffset;
			}
		}

		private void LayoutRow(IList<RectTransform> contents, float rowWidth, float rowHeight, float maxWidth, float xOffset, float yOffset, int axis)
		{
			var xPos = xOffset;

			if (!m_childForceExpandWidth && IsCenterAlign)
			{
				xPos += (maxWidth - rowWidth) * 0.5f;
			}
			else if (!m_childForceExpandWidth && IsRightAlign)
			{
				xPos += maxWidth - rowWidth;
			}

			var extraWidth = 0f;
			var extraSpacing = 0f;

			if (m_childForceExpandWidth)
			{
				extraWidth = (maxWidth - rowWidth) / m_itemList.Count;
			}
			else if (m_flushAlignHorizontal)
			{
				extraSpacing = (maxWidth - rowWidth) / (m_itemList.Count - 1);
				if (m_itemList.Count > 1)
				{
					if (IsCenterAlign)
					{
						xPos -= extraSpacing * 0.5f * (m_itemList.Count - 1);
					}
					else if (IsRightAlign)
					{
						xPos -= extraSpacing * (m_itemList.Count - 1);
					}
				}
			}

			for (var j = 0; j < m_itemList.Count; j++)
			{

				var index = IsLowerAlign ? m_itemList.Count - 1 - j : j;

				var rowChild = m_itemList[index];

				var rowChildWidth = LayoutUtility.GetPreferredSize(rowChild, 0) + extraWidth;
				var rowChildHeight = LayoutUtility.GetPreferredSize(rowChild, 1);

				if (m_childForceExpandHeight)
                {
                    rowChildHeight = rowHeight;
                }

                rowChildWidth = Mathf.Min(rowChildWidth, maxWidth);

				var yPos = yOffset;

				if (IsMiddleAlign)
				{
					yPos += (rowHeight - rowChildHeight) * 0.5f;
				}
				else if (IsLowerAlign)
				{
					yPos += rowHeight - rowChildHeight;
				}

				if (m_flushAlignHorizontal && j > 0)
				{
					xPos += extraSpacing;
				}

				if (axis == 0)
				{
					SetChildAlongAxis(rowChild, 0, xPos, rowChildWidth);
				}
				else
				{
					SetChildAlongAxis(rowChild, 1, yPos, rowChildHeight);
				}

				// Don't do horizontal spacing for the last one
				if (j < m_itemList.Count - 1)
				{
					xPos += rowChildWidth + m_spacing.x;
				}
			}
		}

		private void LayoutCol(IList<RectTransform> contents, float colWidth, float colHeight, float maxHeight, float xOffset, float yOffset, int axis)
		{
			var yPos = yOffset;

			if (!m_childForceExpandHeight && IsMiddleAlign)
			{
				yPos += (maxHeight - colHeight) * 0.5f;
			}
			else if (!m_childForceExpandHeight && IsLowerAlign)
			{
				yPos += maxHeight - colHeight;
			}

			var extraHeight = 0f;
			var extraSpacing = 0f;

			if (m_childForceExpandHeight)
			{
				extraHeight = (maxHeight - colHeight) / m_itemList.Count;
			}
			else if (m_flushAlignHorizontal)
			{
				extraSpacing = (maxHeight - colHeight) / (m_itemList.Count - 1);
				if (m_itemList.Count > 1)
				{
					if (IsMiddleAlign)
					{
						yPos -= extraSpacing * 0.5f * (m_itemList.Count - 1);
					}
					else if (IsLowerAlign)
					{
						yPos -= extraSpacing * (m_itemList.Count - 1);
					}
				}
			}

			for (var j = 0; j < m_itemList.Count; j++)
			{
				var index = IsRightAlign ? m_itemList.Count - 1 - j : j;

				var rowChild = m_itemList[index];

				var rowChildWidth = LayoutUtility.GetPreferredSize(rowChild, 0);
				var rowChildHeight = LayoutUtility.GetPreferredSize(rowChild, 1) + extraHeight;

				if (m_childForceExpandWidth)
				{
					rowChildWidth = colWidth;
				}

				rowChildHeight = Mathf.Min(rowChildHeight, maxHeight);

				var xPos = xOffset;

				if (IsCenterAlign)
				{
					xPos += (colWidth - rowChildWidth) * 0.5f;
				}
				else if (IsRightAlign)
				{
					xPos += colWidth - rowChildWidth;
				}

				if (m_flushAlignHorizontal && j > 0)
				{
					yPos += extraSpacing;
				}

				if (axis == 0)
				{
					SetChildAlongAxis(rowChild, 0, xPos, rowChildWidth);
				}
				else
				{
					SetChildAlongAxis(rowChild, 1, yPos, rowChildHeight);
				}

				// Don't do vertical spacing for the last one
				if (j < m_itemList.Count - 1)
				{
					yPos += rowChildHeight + m_spacing.y;
				}
			}
		}

		private float GetGreatestMinimumChildWidth()
		{
			var max = 0f;
			for (var i = 0; i < rectChildren.Count; i++)
			{
				var w = LayoutUtility.GetMinWidth(rectChildren[i]);

				max = Mathf.Max(w, max);
			}
			return max;
		}

		private float GetGreatestMinimumChildHeight()
		{
			var max = 0f;
			for (var i = 0; i < rectChildren.Count; i++)
			{
				var w = LayoutUtility.GetMinHeight(rectChildren[i]);

				max = Mathf.Max(w, max);
			}
			return max;
		}
	}
}